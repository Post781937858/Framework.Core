using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Framework.QuartzConsole
{
    public class QuartzServer : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            QuartzStartup.StartupAsync().Wait();
            return true;
        }
        public bool Stop(HostControl hostControl)
        {
            SchedulerCenter.GetSchedulerCenter().StopScheduleAsync().Wait();
            return true;
        }
    }
}
