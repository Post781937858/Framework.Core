using Framework.Core.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Five.QuartzNetJob.ExecuteJobTask.Service
{
    public class TestJobTask : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString());
            var schedule = new ScheduleEntity
            {
                JobGroup = context.JobDetail.Key.Group,
                JobName = context.JobDetail.Key.Name,
            };
            await Console.Out.WriteLineAsync(string.Format("任务分组：{0}任务名称：{1}", schedule.JobGroup, schedule.JobName));
        }
    }
}
