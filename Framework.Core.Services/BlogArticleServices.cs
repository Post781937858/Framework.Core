using Framework.Core.IRepository;
using Framework.Core.IServices;
using Framework.Core.Models;
using Framework.Core.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Services
{
    public class BlogArticleService : BaseServices<BlogArticle>, IBlogArticleServices
    {
        public BlogArticleService(IBlogArticleRepository Repository) : base(Repository)
        {

        }
    }
}
