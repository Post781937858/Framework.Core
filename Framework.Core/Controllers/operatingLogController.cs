using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.Core.IServices;
using Framework.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Controllers
{
    [Authorize(Permissions.Name)]
    [Route("api/[controller]")]
    [ApiController]
    public class operatingLogController : ControllerBase
    {
        private readonly IoperatingLogServices ioperatingLogServices;

        public operatingLogController(IoperatingLogServices ioperatingLogServices)
        {
            this.ioperatingLogServices = ioperatingLogServices;
        }


        [HttpGet]
        public async Task<MessageModel<PageModel<operatingLog>>> Query(int Pageindex, int PageSize = 10, string userNumber = "")
        {
            Expression<Func<operatingLog, bool>> whereExpression = r => true;
            if (!string.IsNullOrEmpty(userNumber))
            {
                whereExpression = r => r.UserName == userNumber;
            }
            var data = await ioperatingLogServices.QueryPage(whereExpression, Pageindex, PageSize, "Date desc");
            return new MessageModel<PageModel<operatingLog>>(data);
        }
    }
}