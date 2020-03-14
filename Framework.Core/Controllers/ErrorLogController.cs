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
    public class ErrorLogController : ControllerBase
    {
        private readonly IErrorLogServices _ErrorLogServices;

        public ErrorLogController(IErrorLogServices _ErrorLogServices)
        {
            this._ErrorLogServices = _ErrorLogServices;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<ErrorLog>>> Query(int Pageindex, int PageSize = 10)
        {
            Expression<Func<ErrorLog, bool>> whereExpressionAll = r => true;

            var data = await _ErrorLogServices.QueryPage(whereExpressionAll, Pageindex, PageSize);
            return new MessageModel<PageModel<ErrorLog>>(data);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Add(ErrorLog model)
        {
            model.Id = 0;
            return new MessageModel(await _ErrorLogServices.Add(model) > 0);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel> Update(ErrorLog model)
        {
            return new MessageModel(await _ErrorLogServices.Update(model));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Listmodel"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel> Delete(List<ErrorLog> Listmodel)
        {
            List<object> Ids = new List<object>();
            Listmodel.ForEach(p => Ids.Add(p.Id));
            return new MessageModel(await _ErrorLogServices.DeleteByIds(Ids.ToArray()));
        }
    }
}