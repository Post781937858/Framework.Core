using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.Common;
using Framework.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;

        public ServerController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        [HttpGet]
        public async Task<MessageModel<ServerManage>> Query()
        {
            var server = await Task.Run(() => { return new ServerManage(environment); });
            return new MessageModel<ServerManage>(server);
        }

        [HttpGet("ServerStatus")]
        public async Task<MessageModel<ServerStatus>> QueryServerStatus()
        {
            var server = await Task.Run(() => { return new ServerStatus(); });
            return new MessageModel<ServerStatus>(server);
        }
    }
}