using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using Quartz.Impl.Matchers;
using Framework.Core.Models;
using System.Collections.Concurrent;
using System.Linq;

namespace Framework.QuartzConsole
{
    /// <summary>
	/// 任务调度中心
	/// </summary>
	public class SchedulerCenter
    {
        /// <summary>
        /// 任务调度对象
        /// </summary>
        private static SchedulerCenter SchedulerCenterInstance;

        private IScheduler _scheduler;

        private readonly object SchedulerLock = new object();

        /// <summary>
        /// 任务列表
        /// </summary>
        private ConcurrentDictionary<int, ScheduleEntity> ScheduleList = new ConcurrentDictionary<int, ScheduleEntity>();

        static SchedulerCenter()
        {
            SchedulerCenterInstance = new SchedulerCenter();
        }

        /// <summary>
        /// 获取任务调度中心
        /// </summary>
        /// <returns></returns>
        public static SchedulerCenter GetSchedulerCenter()
        {
            return SchedulerCenterInstance;
        }


        /// <summary>
        /// 返回任务计划（调度器）
        /// </summary>
        /// <returns></returns>
        private async Task<IScheduler> Scheduler()
        {
            if (this._scheduler != null && !this._scheduler.IsShutdown)
            {
                return this._scheduler;
            }
            // 从Factory中获取Scheduler实例
            IScheduler scheduler = await (new StdSchedulerFactory(new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            })).GetScheduler();
            await scheduler.Start();
            lock (SchedulerLock)
            {
                if (this._scheduler != null && !this._scheduler.IsShutdown)
                {
                    return this._scheduler;
                }
                this._scheduler = scheduler;
            }
            return this._scheduler;
        }

        /// <summary>
        /// 运行指定的计划(泛型指定IJob实现类)
        /// </summary>
        /// <param name="jobGroup">任务分组</param>
        /// <param name="jobName">任务名称</param>
        /// <returns></returns>
        public async Task<BaseQuartzNetResult> RunScheduleJob<V>(ScheduleEntity scheduleModel) where V : IJob
        {
            BaseQuartzNetResult result;
            //添加任务
            var addResult = AddScheduleJob<V>(scheduleModel).Result;
            if (addResult.Code == 200)
            {
                //用给定的密钥恢复（取消暂停）IJobDetail
                await (await Scheduler()).ResumeJob(new JobKey(scheduleModel.JobName, scheduleModel.JobGroup));
                result = new BaseQuartzNetResult
                {
                    Code = 200,
                    Msg = "启动成功"
                };
            }
            else
            {
                result = new BaseQuartzNetResult
                {
                    Code = -1,
                    Msg = addResult.Msg
                };
            }
            return result;
        }


        /// <summary>
        /// 运行指定的计划(映射处理IJob实现类)
        /// </summary>
        /// <param name="jobGroup">任务分组</param>
        /// <param name="jobName">任务名称</param>
        /// <returns></returns>
        public async Task<BaseQuartzNetResult> RunScheduleJob(ScheduleEntity scheduleModel)
        {
            BaseQuartzNetResult result;
            //创建指定泛型类型参数指定的类型实例
            //添加任务
            var addResult = await AddScheduleJob(scheduleModel);
            if (addResult.Code == 200)
            {
                //用给定的密钥恢复（取消暂停）IJobDetail
                await (await Scheduler()).ResumeJob(new JobKey(scheduleModel.JobName, scheduleModel.JobGroup));
                result = new BaseQuartzNetResult
                {
                    Code = 200,
                    Msg = "启动成功"
                };
            }
            else
            {
                result = new BaseQuartzNetResult
                {
                    Code = -1,
                    Msg= addResult.Msg
                };
            }
            return result;
        }


        #region 添加任务调度

        /// <summary>
        /// 添加一个工作调度（映射程序集指定IJob实现类）
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private async Task<BaseQuartzNetResult> AddScheduleJob(ScheduleEntity m)
        {
            var result = new BaseQuartzNetResult() { Code = -1, Msg = "" };
            try
            {
                //检查任务是否已存在
                var jk = new JobKey(m.JobName, m.JobGroup);
                if (await (await Scheduler()).CheckExists(jk))
                {
                    //删除已经存在任务
                    await (await Scheduler()).DeleteJob(jk);
                    ScheduleList.Remove(m.Id, out ScheduleEntity value);
                }
                //反射获取任务执行类
                Assembly assembly = Assembly.Load(new AssemblyName(m.AssemblyName));
                Type jobType = assembly.GetTypes().FirstOrDefault(p => p.Name == m.ClassName);
                // 定义这个工作，并将其绑定到我们的IJob实现类
                IJobDetail job = new JobDetailImpl(m.JobName, m.JobGroup, jobType);
                //IJobDetail job = JobBuilder.CreateForAsync<T>().WithIdentity(m.JobName, m.JobGroup).Build();
                // 创建触发器
                ITrigger trigger;
                //校验是否正确的执行周期表达式
                if (!string.IsNullOrEmpty(m.Cron) && CronExpression.IsValidExpression(m.Cron))
                {
                    trigger = CreateCronTrigger(m);
                }
                else
                {
                    trigger = CreateSimpleTrigger(m);
                }
                // 设置监听器
                JobListener listener = new JobListener();
                // IMatcher<JobKey> matcher = KeyMatcher<JobKey>.KeyEquals(job.Key);
                (await Scheduler()).ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.AnyGroup());
                // 告诉Quartz使用我们的触发器来安排作业
                await (await Scheduler()).ScheduleJob(job, trigger);
                ScheduleList.GetOrAdd(m.Id, m);
                result.Code = 200;

            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 添加任务调度（指定IJob实现类）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        private async Task<BaseQuartzNetResult> AddScheduleJob<T>(ScheduleEntity m) where T : IJob
        {
            var result = new BaseQuartzNetResult() { Code = -1, Msg = "" };
            try
            {
                //检查任务是否已存在
                var jk = new JobKey(m.JobName, m.JobGroup);
                if (await (await Scheduler()).CheckExists(jk))
                {
                    //删除已经存在任务
                    await (await Scheduler()).DeleteJob(jk);
                    ScheduleList.Remove(m.Id, out ScheduleEntity value);
                }
                // 定义这个工作，并将其绑定到我们的IJob实现类
                IJobDetail job = JobBuilder.CreateForAsync<T>().WithIdentity(m.JobName, m.JobGroup).Build();
                // 创建触发器
                ITrigger trigger;
                //校验是否正确的执行周期表达式
                if (!string.IsNullOrEmpty(m.Cron) && CronExpression.IsValidExpression(m.Cron))
                {
                    trigger = CreateCronTrigger(m);
                }
                else
                {
                    trigger = CreateSimpleTrigger(m);
                }
                // 设置监听器
                JobListener listener = new JobListener();
                // IMatcher<JobKey> matcher = KeyMatcher<JobKey>.KeyEquals(job.Key);
                (await Scheduler()).ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.AnyGroup());
                // 告诉Quartz使用我们的触发器来安排作业
                await (await Scheduler()).ScheduleJob(job, trigger);
                ScheduleList.GetOrAdd(m.Id, m);
                result.Code = 200;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(string.Format("添加任务出错", ex.Message));
                result.Code = 1001;
                result.Msg = ex.Message;
            }
            return result;
        }

        #endregion


        /// <summary>
        /// 暂停指定的计划
        /// </summary>
        /// <param name="jobGroup">任务分组</param>
        /// <param name="jobName">任务名称</param>
        /// <param name="isDelete">停止并删除任务</param>
        /// <returns></returns>
        public async Task<BaseQuartzNetResult> StopScheduleJob(int jobId)
        {
            var result = new BaseQuartzNetResult() { Code = -1, Msg = "" };
            try
            {
                if (ScheduleList.ContainsKey(jobId))
                {
                    //获取任务实例
                    ScheduleEntity scheduleModel = ScheduleList[jobId];
                    //检查任务是否存在
                    var jk = new JobKey(scheduleModel.JobName, scheduleModel.JobGroup);
                    if (!await (await Scheduler()).CheckExists(jk))
                    {
                        return new BaseQuartzNetResult
                        {
                            Code = -1,
                            Msg = "任务未运行"
                        };
                    }
                    await (await Scheduler()).PauseJob(jk);
                    result = new BaseQuartzNetResult
                    {
                        Code = 200,
                        Msg = "停止任务成功！"
                    };
                }
                else
                {
                    result = new BaseQuartzNetResult
                    {
                        Code = -1,
                        Msg = "不存在该任务"
                    };
                }
            }
            catch (Exception ex)
            {
                result = new BaseQuartzNetResult
                {
                    Code = -1,
                    Msg = "停止任务失败，错误信息：" + ex.Message
                };
            }
            return result;
        }

        /// <summary>
        /// 恢复运行暂停的任务
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobGroup">任务分组</param>
        public async Task<BaseQuartzNetResult> ResumeJob(int jobId)
        {
            var result = new BaseQuartzNetResult() { Code = -1, Msg = "" };
            try
            {
                if (ScheduleList.ContainsKey(jobId))
                {
                    //获取任务实例
                    ScheduleEntity scheduleModel = ScheduleList[jobId];
                    //检查任务是否存在
                    var jk = new JobKey(scheduleModel.JobName, scheduleModel.JobGroup);
                    if (!await (await Scheduler()).CheckExists(jk))
                    {
                        return new BaseQuartzNetResult
                        {
                            Code = -1,
                            Msg = "任务未运行"
                        };
                    }
                    //任务已经存在则恢复运行暂停任务
                    await (await Scheduler()).ResumeJob(jk);
                    result = new BaseQuartzNetResult
                    {
                        Code = 200,
                        Msg = "恢复运行任务成功！"
                    };
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(string.Format("恢复任务失败！{0}", ex));
                result = new BaseQuartzNetResult
                {
                    Code = -1,
                    Msg = "恢复运行任务失败"
                };
            }
            return result;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<BaseQuartzNetResult> RemoveJob(int jobId)
        {
            var result = new BaseQuartzNetResult() { Code = -1, Msg = "" };
            if (ScheduleList.ContainsKey(jobId))
            {
                //获取任务实例
                ScheduleEntity scheduleModel = ScheduleList[jobId];
                //检查任务是否已存在
                var jk = new JobKey(scheduleModel.JobName, scheduleModel.JobGroup);
                if (await (await Scheduler()).CheckExists(jk))
                {
                    //删除已经存在任务
                    var res = await (await Scheduler()).DeleteJob(jk);
                    if (res)
                        ScheduleList.Remove(jobId, out scheduleModel);
                    result = new BaseQuartzNetResult
                    {
                        Code = res ? 200 : -1,
                        Msg = res ? "删除任务成功" : "删除任务失败"
                    };
                }
                else
                {
                    result = new BaseQuartzNetResult
                    {
                        Code = 500,
                        Msg = "调度器中不存在该任务"
                    };
                }
            }
            else
            {
                result = new BaseQuartzNetResult
                {
                    Code = 500,
                    Msg = "不存在该任务"
                };
            }
            return result;
        }


        /// <summary>
        /// 停止任务调度
        /// </summary>
        public async Task<BaseQuartzNetResult> StopScheduleAsync()
        {
            var result = new BaseQuartzNetResult() { Code = -1, Msg = "" };
            try
            {
                //判断调度是否已经关闭
                if (!(await Scheduler()).IsShutdown)
                {
                    //等待任务运行完成
                    await (await Scheduler()).Shutdown();
                }
                result = new BaseQuartzNetResult
                {
                    Code = 200,
                    Msg = "任务调度停止成功"
                };
                await Console.Out.WriteLineAsync("任务调度停止！");
            }
            catch (Exception ex)
            {
                result = new BaseQuartzNetResult
                {
                    Code = -1,
                    Msg = "任务调度停止失败，错误信息：" + ex.Message
                };
            };
            return result;
        }

        /// <summary>
        /// 创建类型Simple的触发器
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private ITrigger CreateSimpleTrigger(ScheduleEntity m)
        {
            //作业触发器
            if (m.RunTimes > 0)
            {
                return TriggerBuilder.Create()
               .WithIdentity(m.JobName, m.JobGroup)
               //.StartAt(m.BeginTime)//开始时间
               //.EndAt(m.EndTime)//结束时间
               .WithSimpleSchedule(x => x
                   .WithIntervalInSeconds(m.IntervalSecond)//执行时间间隔，单位秒
                   .WithRepeatCount(m.RunTimes))//执行次数、默认从0开始
                   .ForJob(m.JobName, m.JobGroup)//作业名称
               .Build();
            }
            else
            {
                return TriggerBuilder.Create()
               .WithIdentity(m.JobName, m.JobGroup)
               //.StartAt(m.BeginTime)//开始时间
               //.EndAt(m.EndTime)//结束时间
               .WithSimpleSchedule(x => x
                   .WithIntervalInSeconds(m.IntervalSecond)//执行时间间隔，单位秒
                   .RepeatForever())//无限循环
                   .ForJob(m.JobName, m.JobGroup)//作业名称
               .Build();
            }

        }
        /// <summary>
        /// 创建类型Cron的触发器
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private ITrigger CreateCronTrigger(ScheduleEntity m)
        {
            // 作业触发器
            return TriggerBuilder.Create()
                   .WithIdentity(m.JobName, m.JobGroup)
                   //.StartAt(m.BeginTime)//开始时间
                   //.EndAt(m.EndTime)//结束数据
                   .WithCronSchedule(m.Cron)//指定cron表达式
                   .ForJob(m.JobName, m.JobGroup)//作业名称
                   .Build();
        }
    }
}
