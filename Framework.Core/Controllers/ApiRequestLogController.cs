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
    public class ApiRequestLogController : ControllerBase
    {
        private readonly IApiRequestLogServices _ApiRequestLogServices;

        public ApiRequestLogController(IApiRequestLogServices _ApiRequestLogServices)
        {
            this._ApiRequestLogServices = _ApiRequestLogServices;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<ApiRequestLog>>> Query(string path, Requeststate state, string userName, int Pageindex, int PageSize = 10)
        {
            Expression<Func<ApiRequestLog, bool>> whereExpressionAll = r => true;
            if (!string.IsNullOrEmpty(path))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.path == path);
            }
            if ((int)state != 0)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.state == state);
            }
            if (!string.IsNullOrEmpty(userName))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.userName == userName);
            }
            var data = await _ApiRequestLogServices.QueryPage(whereExpressionAll, Pageindex, PageSize, "requestTime desc");
            return new MessageModel<PageModel<ApiRequestLog>>(data);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Add(ApiRequestLog model)
        {
            model.Id = 0;
            return new MessageModel(await _ApiRequestLogServices.Add(model) > 0);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel> Update(ApiRequestLog model)
        {
            return new MessageModel(await _ApiRequestLogServices.Update(model));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Listmodel"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel> Delete(List<ApiRequestLog> Listmodel)
        {
            List<object> Ids = new List<object>();
            Listmodel.ForEach(p => Ids.Add(p.Id));
            return new MessageModel(await _ApiRequestLogServices.DeleteByIds(Ids.ToArray()));
        }
    }
}