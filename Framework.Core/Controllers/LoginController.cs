using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.IServices;
using Framework.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IOrderTaskServices services;

        public LoginController(IOrderTaskServices services)
        {
            this.services = services;
        }

        /// <summary>
        /// 获取Token 无权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OkObjectResult GetToken()
        {
            return Ok(new
            {
                success = true,
                data = new { token = JWTTokenService.GetToken(new TokenModelJwt() { Uid = 1, Name = "张三", Role = "admin" }) }
            });
        }
    }
}