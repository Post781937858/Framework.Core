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
using Framework.Core.Common;
using System.Diagnostics;
using Framework.Core.IServices;
using Framework.Core.Models;
using System.Net;

namespace Framework.Core.Middlewares
{
    /// <summary>
    /// 中间件
    /// 记录请求和响应数据
    /// </summary>
    public class RequRespLogMildd
    {
        private readonly RequestDelegate _next;
        private readonly IApiRequestLogServices requestLogServices;
        private readonly IUser user;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public RequRespLogMildd(RequestDelegate next, IApiRequestLogServices requestLogServices, IUser user)
        {
            _next = next;
            this.requestLogServices = requestLogServices;
            this.user = user;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (Appsettings.app("Middleware", "RequestResponseLog", "Enabled").ToBool())
            {
                // 过滤，只有接口
                if (context.Request.Path.Value.Contains("api") && !context.Request.QueryString.ToString().Contains("Page"))
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    context.Request.EnableBuffering();
                    Stream originalBody = context.Response.Body;
                    string Path = context.Request.Path + context.Request.QueryString;
                    string DataRequest = string.Empty;
                    string DataResponse = string.Empty;
                    try
                    {
                        DataRequest = await RequestData(context);

                        using (var ms = new MemoryStream())
                        {
                            context.Response.Body = ms;

                            await _next(context);

                            DataResponse = ResponseData(context.Response, ms);

                            ms.Position = 0;
                            await ms.CopyToAsync(originalBody);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        context.Response.Body = originalBody;
                    }
                    stopwatch.Stop();
                    Parallel.For(0, 1, s =>
                    {
                        ApiRequestLog requestLog = new ApiRequestLog()
                        {
                            userName = user.Name,
                            consumingTime = stopwatch.ElapsedMilliseconds,
                            method= context.Request.Method.ToLower(),
                            FormDataparameter = DataRequest,
                            path = context.Request.Path,
                            Urlparameter = context.Request.QueryString.ToString(),
                            ResponseData = DataResponse,
                            state = context.Response.StatusCode == StatusCodes.Status200OK ? Requeststate.succeed : Requeststate.error,
                            requestTime = DateTime.Now
                        };
                        requestLogServices.Add(requestLog);
                    });
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task<string> RequestData(HttpContext context)
        {
            var request = context.Request;
            var sr = new StreamReader(request.Body);

            var content = await sr.ReadToEndAsync();
            request.Body.Position = 0;
            return content;
        }

        private string ResponseData(HttpResponse response, MemoryStream ms)
        {
            ms.Position = 0;
            var ResponseBody = new StreamReader(ms).ReadToEnd();

            // 去除 Html
            var reg = "<[^>]+>";
            var isHtml = Regex.IsMatch(ResponseBody, reg);

            return ResponseBody;
        }
    }
}

