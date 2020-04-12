using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Common;
using Framework.Core.IServices;
using Framework.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Framework.Core.QuartzServices;

namespace Framework.Core.Controllers
{
    [Authorize(Permissions.Name)]
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleEntityController : ControllerBase
    {
        private readonly IScheduleEntityServices _ScheduleEntityServices;
        private readonly QuartzServicesClient quartzServicesClient;

        public ScheduleEntityController(IScheduleEntityServices _ScheduleEntityServices, QuartzServicesClient quartzServicesClient)
        {
            this._ScheduleEntityServices = _ScheduleEntityServices;
            this.quartzServicesClient = quartzServicesClient;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<ScheduleEntity>>> Query(String JobName, String JobGroup, int Pageindex, int PageSize = 10)
        {
            Expression<Func<ScheduleEntity, bool>> whereExpressionAll = r => true;
            if (!string.IsNullOrEmpty(JobName))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.JobName == JobName);
            }

            if (!string.IsNullOrEmpty(JobGroup))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.JobGroup == JobGroup);
            }
            var data = await _ScheduleEntityServices.QueryPage(whereExpressionAll, Pageindex, PageSize);
            return new MessageModel<PageModel<ScheduleEntity>>(data);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Add(ScheduleEntity model)
        {
            model.Id = 0;
            model.RunStatus = JobRunStatus.Await;
            return new MessageModel(await _ScheduleEntityServices.Add(model) > 0);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel> Update(ScheduleEntity model)
        {
            if (model.IntervalSecond == 0)
            {
                model.IntervalSecond = 1;
            }
            return new MessageModel(await _ScheduleEntityServices.Update(model));
        }

        /// <summary>
        /// 运行Job
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Run")]
        public async Task<MessageModel> RunJobTask(ScheduleEntity model)
        {
            var result = await quartzServicesClient.RunScheduleJobAsync(new ScheduleModel { JonId = model.Id });
            if (result.Code == 200)
            {
                model.RunStatus = JobRunStatus.run;
                await _ScheduleEntityServices.Update(model);
            }
            return new MessageModel(result.Code == 200, result.Msg);
        }

        /// <summary>
        /// 停止Job
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Stop")]
        public async Task<MessageModel> StopobTask(ScheduleEntity model)
        {
            var result = await quartzServicesClient.StopScheduleJobAsync(new ScheduleModel { JonId = model.Id });
            if (result.Code == 200)
            {
                model.RunStatus = JobRunStatus.stop;
                await _ScheduleEntityServices.Update(model);
            }
            return new MessageModel(result.Code == 200, result.Msg);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Listmodel"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel> Delete(List<ScheduleEntity> Listmodel)
        {
            bool Result = true;
            foreach (var item in Listmodel)
            {
                var result = await quartzServicesClient.RemoveJobAsync(new ScheduleModel { JonId = item.Id });
                if (result.Code == 200 || result.Code == 500)
                {
                    var res = await _ScheduleEntityServices.DeleteById(item.Id);
                    if (!res)
                    {
                        Result = false;
                        break;
                    }
                }else
                    Result = false;
                break;
            }
            return new MessageModel(Result);
        }
    }
}