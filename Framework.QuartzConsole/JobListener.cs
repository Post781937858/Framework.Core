using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.QuartzConsole
{
    /// <summary>
    /// 任务监听器
    /// </summary>
    public class JobListener : IJobListener
    {
        public string Name => "JobListener";

        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            await Console.Out.WriteLineAsync("");
        }

        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            await Console.Out.WriteLineAsync("");
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            await Console.Out.WriteLineAsync("");
        }
    }
}
