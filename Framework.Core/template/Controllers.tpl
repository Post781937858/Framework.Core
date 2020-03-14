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
    public class {t_object}Controller : ControllerBase
    {
        private readonly I{t_object}Services _{t_object}Services;

        public {t_object}Controller(I{t_object}Services _{t_object}Services)
        {
            this._{t_object}Services = _{t_object}Services;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<{t_object}>>> Query(int Pageindex, int PageSize = 10)
        {
            Expression<Func<{t_object}, bool>> whereExpressionAll = r => true;

            var data = await _{t_object}Services.QueryPage(whereExpressionAll, Pageindex, PageSize);
            return new MessageModel<PageModel<{t_object}>>(data);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Add({t_object} model)
        {
            model.Id = 0;
            return new MessageModel(await _{t_object}Services.Add(model) > 0);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel> Update({t_object} model)
        {
            return new MessageModel(await _{t_object}Services.Update(model));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Listmodel"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel> Delete(List<{t_object}> Listmodel)
        {
            List<object> Ids = new List<object>();
            Listmodel.ForEach(p => Ids.Add(p.Id));
            return new MessageModel(await _{t_object}Services.DeleteByIds(Ids.ToArray()));
        }
    }
}