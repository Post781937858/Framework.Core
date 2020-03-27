using Framework.Core.Common;
using Framework.Core.Models;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework.QuartzConsole
{
    public class QuartzStartup
    {
        public static async Task StartupAsync()
        {
            try
            {
                Server _server = new Server
                {
                    Services = { QuartzServices.BindService(new QuartzJobService()) },
                    Ports = { new ServerPort("localhost", 40001, ServerCredentials.Insecure) }
                };
                _server.Start();
                var Db = DBClientManage.GetSqlSugarClient();
                var schedule = Db.Queryable<ScheduleEntity>().Where(w => w.RunStatus == JobRunStatus.run).ToList();
                foreach (var item in schedule)
                {
                    if (!string.IsNullOrEmpty(item.AssemblyName) && !string.IsNullOrEmpty(item.ClassName))
                    {
                        await SchedulerCenter.GetSchedulerCenter().RunScheduleJob(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
