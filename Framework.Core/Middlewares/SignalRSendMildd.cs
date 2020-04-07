using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using Framework.Core.Common;

namespace Blog.Core.Middlewares
{
    /// <summary>
    /// 中间件
    /// 记录请求和响应数据
    /// </summary>
    public class SignalRSendMildd
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //private readonly RequestDelegate _next;
        //private readonly IHubContext<ChatHub> _hubContext;

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="next"></param>
        ///// <param name="hubContext"></param>
        //public SignalRSendMildd(RequestDelegate next, IHubContext<ChatHub> hubContext)
        //{
        //    _next = next;
        //    _hubContext = hubContext;
        //}



        //public async Task InvokeAsync(HttpContext context)
        //{
        //    if (Appsettings.app("Middleware", "SignalR", "Enabled").ObjToBool())
        //    {
        //        await _hubContext.Clients.All.SendAsync("ReceiveUpdate", LogLock.GetLogData()); 
        //    }
        //    await _next(context);
        //}

    }
}

