using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Common;
using Framework.Core.IServices;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MianStatisticsViewController : ControllerBase
    {
        private readonly IMianStatisticsViewServices _MianStatisticsViewServices;

        public MianStatisticsViewController(IMianStatisticsViewServices _MianStatisticsViewServices)
        {
            this._MianStatisticsViewServices = _MianStatisticsViewServices;
        }


        [HttpGet]
        public async Task<MessageModel<MianStatisticsView>> QueryChart()
        {
            var data = await _MianStatisticsViewServices.GetMainStatisticsViewAsync();
            return new MessageModel<MianStatisticsView>(data);
        }


        [HttpGet("QueryTag")]
        public async Task<MessageModel<MianStatisticsView>> QueryTag()
        {
            var data = await _MianStatisticsViewServices.GetTagViewAsync();
            return new MessageModel<MianStatisticsView>(data);
        }
    }
}