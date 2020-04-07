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
    public class BlogLinksController : ControllerBase
    {
        private readonly IBlogLinksServices _BlogLinksServices;

        public BlogLinksController(IBlogLinksServices _BlogLinksServices)
        {
            this._BlogLinksServices = _BlogLinksServices;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<BlogLinks>>> Query(string LinkName, string LinkUrl, int Status, int Pageindex, int PageSize = 10)
        {
            Expression<Func<BlogLinks, bool>> whereExpressionAll = r => true;
            if (!string.IsNullOrEmpty(LinkName))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.LinkName == LinkName);
            }
            if (!string.IsNullOrEmpty(LinkUrl))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.LinkUrl == LinkUrl);
            }
            if ((int)Status != 0)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.Status == Status);
            }
            var data = await _BlogLinksServices.QueryPage(whereExpressionAll, Pageindex, PageSize);
            return new MessageModel<PageModel<BlogLinks>>(data);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Add(BlogLinks model)
        {
            model.Id = 0;
            return new MessageModel(await _BlogLinksServices.Add(model) > 0);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel> Update(BlogLinks model)
        {
            return new MessageModel(await _BlogLinksServices.Update(model));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Listmodel"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel> Delete(List<BlogLinks> Listmodel)
        {
            List<object> Ids = new List<object>();
            Listmodel.ForEach(p => Ids.Add(p.Id));
            return new MessageModel(await _BlogLinksServices.DeleteByIds(Ids.ToArray()));
        }
    }
}