using Blog.Core.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Framework.Core.Middlewares
{
    public static class MiddlewareHelpers
    {
        /// <summary>
        /// 请求响应中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseReuestResponseLog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequRespLogMildd>();
        }

        /// <summary>
        /// SignalR中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSignalRSendMildd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SignalRSendMildd>();
        }

        /// <summary>
        /// 异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionHandlerMidd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMidd>();
        }

        /// <summary>
        /// IP请求中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static void UseIPLogMildd(this IApplicationBuilder app)
        {
            app.UseMiddleware<IPLogMildd>();
        }

        /// <summary>
        /// 注册所有中间件
        /// </summary>
        /// <returns></returns>
        public static void UseMiddlewareAll(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequRespLogMildd>();
            //app.UseMiddleware<SignalRSendMildd>();
            app.UseMiddleware<ExceptionHandlerMidd>();
            app.UseMiddleware<IPLogMildd>();
        }
    }
}
