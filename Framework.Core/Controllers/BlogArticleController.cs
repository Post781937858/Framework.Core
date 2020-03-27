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
    public class BlogArticleController : ControllerBase
    {
        private readonly IBlogArticleServices _BlogArticleServices;

        public BlogArticleController(IBlogArticleServices _BlogArticleServices)
        {
            this._BlogArticleServices = _BlogArticleServices;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<BlogArticle>>> Query(String bsubmitter, String btitle, String bcategory, Int32 bcommentNum, int Pageindex, int PageSize = 10)
        {
            Expression<Func<BlogArticle, bool>> whereExpressionAll = r => true;
            if (!string.IsNullOrEmpty(bsubmitter))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.bsubmitter == bsubmitter);
            }

            if (!string.IsNullOrEmpty(btitle))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.btitle == btitle);
            }

            if (!string.IsNullOrEmpty(bcategory))
            {
                whereExpressionAll = whereExpressionAll.And(p => p.bcategory == bcategory);
            }

            if ((int)bcommentNum != 0)
            {
                whereExpressionAll = whereExpressionAll.And(p => p.bcommentNum == bcommentNum);
            }

            var data = await _BlogArticleServices.QueryPage(whereExpressionAll, Pageindex, PageSize);
            return new MessageModel<PageModel<BlogArticle>>(data);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Add(BlogArticle model)
        {
            model.Id = 0;
            return new MessageModel(await _BlogArticleServices.Add(model) > 0);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel> Update(BlogArticle model)
        {
            return new MessageModel(await _BlogArticleServices.Update(model));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Listmodel"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel> Delete(List<BlogArticle> Listmodel)
        {
            List<object> Ids = new List<object>();
            Listmodel.ForEach(p => Ids.Add(p.Id));
            return new MessageModel(await _BlogArticleServices.DeleteByIds(Ids.ToArray()));
        }
    }
}