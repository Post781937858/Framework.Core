using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.Common;
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
        private readonly IUserServices services;
        private readonly ICache cache;
        private readonly PermissionRequirement requirement;
        private readonly IoperatingLogServices ioperatingLogServices;

        public LoginController(ILogger<LoginController>  logger, IUserServices services,ICache cache, PermissionRequirement requirement, IoperatingLogServices ioperatingLogServices)
        {
            this.logger = logger;
            this.services = services;
            this.cache = cache;
            this.requirement = requirement;
            this.ioperatingLogServices = ioperatingLogServices;
        }

        /// <summary>
        /// 获取Token 无权限
        /// </summary>
        /// <returns></returns>
        //[Authorize(Permissions.Name)]
        [HttpGet("GetToken")]
        public MessageModel<string> GetToken()
        {
            return new MessageModel<string>(JWTTokenService.GetToken(new TokenModelJwt() { Uid = 1, Name = "张三", Role = "admin" }));
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<object>> LoginAsync(User user)
        {
            string Key = $"errorcount_{user.UserNumber}";
            int errorcount = cache.GetValue(Key).ToInt();
            if (errorcount >= 10)
            {
                return new MessageModel<object>(string.Empty, false, "失败次数过多，请五分钟后再试");
            }
            var ipaddress = HttpContext.Connection.RemoteIpAddress.ToIPv4String();
            var userAgent = HttpContext.Request.Headers["User-Agent"];
            var agent = new UserAgent(userAgent);
            var Browser = $"{agent.Browser?.Name} {agent.Browser?.Version}";
            var OS = $"{agent.OS?.Name} {agent.OS?.Version}";
            var Md5Password = MD5Helper.MD5Encrypt32(user.Password);
            var User = (await services.Query(p => p.UserNumber == user.UserNumber && p.Password == Md5Password)).FirstOrDefault();
            await ioperatingLogServices.Add(new operatingLog
            {
                Operating = "登录",
                Date = DateTime.Now,
                UserName = user.UserNumber,
                ip = ipaddress,
                Browser = Browser,
                OS = OS,
                state = User != null && User.UserState == 200 ? 200 : 500,
                Details = User != null && User.UserState == 200 ? "通过登录授权" : "未通过登录授权"
            });
            if (User != null && User.UserState == 200)
            {
                requirement.Permissions = new List<Models.ViewModels.PermissionItemView>();
                if (errorcount != 0) cache.Remove(Key);
                User.Password = string.Empty;
                return new MessageModel<object>(new { user = User, Token = JWTTokenService.GetToken(new TokenModelJwt() { Uid = User.Id, Name = User.showName, Role = User.PowerName }) });
            }
            else
            {
                errorcount++;
                cache.Set(Key, errorcount, TimeSpan.FromMinutes(5));
                return new MessageModel<object>(null, false, (User != null && User.UserState == 500) ? "账户已冻结" : "账户或密码错误，登录失败");
            }
        }
    }
}