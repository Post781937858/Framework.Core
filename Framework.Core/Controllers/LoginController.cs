using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.IRepository;
using Framework.Core.IServices;
using Framework.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Framework.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> logger;

        public LoginController(ILogger<LoginController>  logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 获取Token 无权限
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        [HttpGet]
        public MessageModel<dynamic> GetToken()
        {
            return new MessageModel<dynamic>(JWTTokenService.GetToken(new TokenModelJwt() { Uid = 1, Name = "张三", Role = "admin" }));
        }



    }
}