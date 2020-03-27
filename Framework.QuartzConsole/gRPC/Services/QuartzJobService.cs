using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.Common;
using Framework.Core.Models;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Framework.QuartzConsole
{
    public class QuartzJobService : QuartzServices.QuartzServicesBase
    {
        public override async Task<QuartzNetResult> RunScheduleJob(ScheduleModel schedule, ServerCallContext context)
        {
            var result = new QuartzNetResult() { Code = -1, Msg = "Ê§°Ü" };
            var Db = DBClientManage.GetSqlSugarClient();
            var Schedule = await Db.Queryable<ScheduleEntity>().FirstAsync(w => w.Id == schedule.JonId);
            if (Schedule != null)
            {
                BaseQuartzNetResult baseQuartz = await SchedulerCenter.GetSchedulerCenter().RunScheduleJob(Schedule);
                result.Code = baseQuartz.Code;
                result.Msg = baseQuartz.Msg;
            }
            return result;
        }

        public override async Task<QuartzNetResult> StopScheduleJob(ScheduleModel schedule, ServerCallContext context)
        {
            var result = new QuartzNetResult();
            BaseQuartzNetResult baseQuartz = await SchedulerCenter.GetSchedulerCenter().StopScheduleJob(schedule.JonId);
            result.Code = baseQuartz.Code;
            result.Msg = baseQuartz.Msg;
            return result;
        }

        public override async Task<QuartzNetResult> RemoveJob(ScheduleModel schedule, ServerCallContext context)
        {
            var result = new QuartzNetResult();
            BaseQuartzNetResult baseQuartz = await SchedulerCenter.GetSchedulerCenter().RemoveJob(schedule.JonId);
            result.Code = baseQuartz.Code;
            result.Msg = baseQuartz.Msg;
            return result;
        }
        public override async Task<QuartzNetResult> StopScheduleAsync(ScheduleModel schedule, ServerCallContext context)
        {
            var result = new QuartzNetResult();
            BaseQuartzNetResult baseQuartz = await SchedulerCenter.GetSchedulerCenter().StopScheduleAsync();
            result.Code = baseQuartz.Code;
            result.Msg = baseQuartz.Msg;
            return result;
        }
    }
}
