using Framework.Core.Common;
using Framework.Core.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework.Core
{
    /// <summary>
    /// 权限授权处理器
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        private readonly IHttpContextAccessor _accessor;
        private readonly IMenuServices menuServices;
        private readonly IUser user;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="accessor"></param>
        /// <param name="menuServices"></param>
        /// <param name="user"></param>
        public PermissionHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor accessor, IMenuServices menuServices, IUser user)
        {
            _accessor = accessor;
            this.menuServices = menuServices;
            this.user = user;
            Schemes = schemes;
        }

        // 重写异步处理程序
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {

            var httpContext = _accessor.HttpContext;

            if (!requirement.Permissions.Any())
            {
                var data = await menuServices.PermissionItemViewsAsync(null);
                requirement.Permissions = data;
            }

            //请求Url
            if (httpContext != null)
            {
                var questUrl = httpContext.Request.Path.Value.ToLower();

                //判断请求是否停止
                var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
                foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
                {
                    if (await handlers.GetHandlerAsync(httpContext, scheme.Name) is IAuthenticationRequestHandler handler && await handler.HandleRequestAsync())
                    {
                        context.Fail();
                        return;
                    }
                }
                //判断请求是否拥有凭据，即有没有登录
                var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
                if (defaultAuthenticate != null)
                {
                    var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                    //result?.Principal不为空即登录成功
                    if (result?.Principal != null)
                    {
                        httpContext.User = result.Principal;
                        var method = httpContext.Request.Method.ToLower();
                        var PermissionsList = requirement.Permissions.Where(w => w.Url?.ToLower() == questUrl && w.method == method && w.Role == user.Role);
                        //权限中是否存在请求的url
                        if (PermissionsList.Count() > 0)
                        {
                            context.Succeed(requirement);
                            return;
                        }
                    }
                    //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
                    if (!questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType))
                    {
                        context.Fail();
                        return;
                    }
                }
                context.Succeed(requirement);
            }
        }
    }
}
