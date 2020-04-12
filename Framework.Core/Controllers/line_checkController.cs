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

namespace Framework.Core.Controllers
{
    [Authorize(Permissions.Name)]
    [Route("api/[controller]")]
    [ApiController]
    public class line_checkController : ControllerBase
    {
        private readonly Iline_checkServices _line_checkServices;

        public line_checkController(Iline_checkServices _line_checkServices)
        {
            this._line_checkServices = _line_checkServices;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<line_check>>> Query(string box_id, string wms_id, string area_code, int size, tasksstate state, string messageName, string location, int weight, sendstate sendSwitch, string source_code, int Pageindex, int PageSize = 10)
        {
            Expression<Func<line_check, bool>> whereExpressionAll = r => true;
            if (!string.IsNullOrEmpty(box_id))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.box_id == box_id);
            }

            if (!string.IsNullOrEmpty(wms_id))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.wms_id == wms_id);
            }

            if (!string.IsNullOrEmpty(area_code))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.area_code == area_code);
            }

            if ((int)size != 0)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.size == size);
            }
            if ((int)state != 99)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.state == state);
            }
            if (!string.IsNullOrEmpty(messageName))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.messageName == messageName);
            }

            if (!string.IsNullOrEmpty(location))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.location == location);
            }

            if ((int)weight != 0)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.weight == weight);
            }
            if ((int)sendSwitch != 99)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.sendSwitch == sendSwitch);
            }
            if (!string.IsNullOrEmpty(source_code))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.source_code == source_code);
            }


            var data = await _line_checkServices.QueryPage(whereExpressionAll, Pageindex, PageSize, "create_time desc");
            return new MessageModel<PageModel<line_check>>(data);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Add(line_check model)
        {
            model.Id = 0;
            return new MessageModel(await _line_checkServices.Add(model) > 0);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel> Update(line_check model)
        {
            return new MessageModel(await _line_checkServices.Update(model));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Listmodel"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel> Delete(List<line_check> Listmodel)
        {
            List<object> Ids = new List<object>();
            Listmodel.ForEach(p => Ids.Add(p.Id));
            return new MessageModel(await _line_checkServices.DeleteByIds(Ids.ToArray()));
        }
    }
}