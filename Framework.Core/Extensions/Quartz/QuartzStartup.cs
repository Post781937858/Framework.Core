using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Extensions.Quartz
{
    public class QuartzStartup
    {
        private readonly ISchedulerFactory schedulerFactory;
        private readonly IJobFactory jobFactory;

        public QuartzStartup(ISchedulerFactory _schedulerFactory, IJobFactory jobFactory)
        {
            schedulerFactory = _schedulerFactory;
            this.jobFactory = jobFactory;
        }

        public  async Task Start()
        {
            ////1、声明一个调度工厂
            //schedulerFactory = new StdSchedulerFactory();
            //2、通过调度工厂获得调度器
            IScheduler _scheduler = await schedulerFactory.GetScheduler();
            _scheduler.JobFactory = jobFactory;
             //3、开启调度器
             await _scheduler.Start();
            //4、创建一个触发器
            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())//每两秒执行一次
                            .Build();
            //5、创建任务
            var jobDetail = JobBuilder.Create<MessageToWebSocketJob>()
                            .WithIdentity("job", "group")
                            .Build();
            //6、将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
