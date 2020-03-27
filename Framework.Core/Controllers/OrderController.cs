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
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _OrderServices;

        public OrderController(IOrderServices _OrderServices)
        {
            this._OrderServices = _OrderServices;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<Order>>> Query(Int32 mesid, String messageName, Int32 boxId, String level, Double weight, String areaCode, String sourceCode, String s_station, String d_station, String location, Int32 wmsid, Int32 state, Int32 priority, DateTime createTime, DateTime endTime, Int32 Id, int Pageindex, int PageSize = 10)
        {
            Expression<Func<Order, bool>> whereExpressionAll = r => true;
            if (mesid != 0)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.mesid == mesid);
            }
            if (!string.IsNullOrEmpty(messageName))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.messageName == messageName);
            }

            if (boxId != 0)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.boxId == boxId);
            }
            if (!string.IsNullOrEmpty(level))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.level == level);
            }
            if (!string.IsNullOrEmpty(areaCode))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.areaCode == areaCode);
            }

            if (!string.IsNullOrEmpty(sourceCode))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.sourceCode == sourceCode);
            }

            if (!string.IsNullOrEmpty(s_station))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.s_station == s_station);
            }

            if (!string.IsNullOrEmpty(d_station))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.d_station == d_station);
            }

            if (!string.IsNullOrEmpty(location))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.location == location);
            }

            if (wmsid != 0)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.wmsid == wmsid);
            }
            if (state != 0)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.state == state);
            }
            if (priority != 0)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.priority == priority);
            }
            if (Id != 0)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.Id == Id);
            }

            var data = await _OrderServices.QueryPage(whereExpressionAll, Pageindex, PageSize);
            return new MessageModel<PageModel<Order>>(data);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Add(Order model)
        {
            model.Id = 0;
            return new MessageModel(await _OrderServices.Add(model) > 0);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel> Update(Order model)
        {
            return new MessageModel(await _OrderServices.Update(model));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Listmodel"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel> Delete(List<Order> Listmodel)
        {
            List<object> Ids = new List<object>();
            Listmodel.ForEach(p => Ids.Add(p.Id));
            return new MessageModel(await _OrderServices.DeleteByIds(Ids.ToArray()));
        }
    }
}