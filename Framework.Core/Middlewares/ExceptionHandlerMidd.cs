using Framework.Core.Common;
using Framework.Core.IServices;
using Framework.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Framework.Core.Middlewares
{
    public class ExceptionHandlerMidd
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMidd> _logger;
        private readonly IErrorLogServices errorLogServices;
        private readonly IUser user;

        public ExceptionHandlerMidd(RequestDelegate next, ILogger<ExceptionHandlerMidd> logger, IErrorLogServices errorLogServices, IUser user)
        {
            this._next = next;
            this._logger = logger;
            this.errorLogServices = errorLogServices;
            this.user = user;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            if (e == null) return;

            _logger.LogError(e, e.GetBaseException().ToString());

            await WriteExceptionAsync(context, e).ConfigureAwait(false);
        }

        private async Task WriteExceptionAsync(HttpContext context, Exception e)
        {
            if (e is UnauthorizedAccessException)
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            else if (e is Exception)
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(new MessageModel(false, $"{e.Message}").ToJson());
            Parallel.For(0, 1, s =>
            {
                ErrorLog errorLog = new ErrorLog()
                {
                    UserId = user.ID,
                    UserName = user.Name,
                    time = DateTime.Now,
                    url = context.Request.Path.ToString(),
                    errorstack= e.StackTrace,
                    errormsg = e.Message
                };
                errorLogServices.Add(errorLog);
            });
        }
    }
}
