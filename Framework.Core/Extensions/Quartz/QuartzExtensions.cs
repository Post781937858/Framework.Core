using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Extensions.Quartz
{
    public static class QuartzExtensions
    {
        public static IServiceCollection AddQuartzManager(this IServiceCollection services)
        {
            services.AddSingleton<QuartzStartup>();
            services.AddScoped<MessageToWebSocketJob>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IJobFactory, IOCJobFactory>();
            return services;
        }

        public static IApplicationBuilder QuartzManager(this IApplicationBuilder app)
        {
            var quartz = app.ApplicationServices.GetRequiredService<QuartzStartup>();
            quartz.Start().Wait();
            return app;
        }
    }
}
