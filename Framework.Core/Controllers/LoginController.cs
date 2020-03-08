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

        public LoginController(ILogger<LoginController>  logger, IUserServices services,ICache cache, PermissionRequirement requirement)
        {
            this.logger = logger;
            this.services = services;
            this.cache = cache;
            this.requirement = requirement;
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
        public async Task<MessageModel<string>> LoginAsync(User user)
        {
            string Key = $"User{user.UserNumber}";
            int errorcount = cache.GetValue(Key).ToInt();
            if (errorcount >= 10)
            {
                return new MessageModel<string>(string.Empty, false, "失败次数过多，请五分钟后再试");
            }
            var Md5Password = MD5Helper.MD5Encrypt32(user.Password);
            var User = (await services.Query(p => p.UserNumber == user.UserNumber && p.Password == Md5Password)).FirstOrDefault();
            if (User != null && User.UserState == 200)
            {
                requirement.Permissions = new List<Models.ViewModels.PermissionItemView>();
                if (errorcount != 0) cache.Remove(Key);
                return new MessageModel<string>(JWTTokenService.GetToken(new TokenModelJwt() { Uid = User.Id, Name = User.showName, Role = User.PowerName }));
            }
            else
            {
                errorcount++;
                cache.Set(Key, errorcount, TimeSpan.FromMinutes(5));
                return new MessageModel<string>(string.Empty, false, (User != null && User.UserState == 500) ? "账户已冻结" : "密码错误，登录失败");
            }
        }
    }
}