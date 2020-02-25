using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Framework.Core.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SqlSugar;
using Swashbuckle.AspNetCore.Filters;

namespace Framework.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new Appsettings(Configuration));
            services.AddScoped<ICache, MemoryCaching>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });
            services.AddScoped<SqlSugar.ISqlSugarClient>(p => new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig()
            {
                ConnectionString = DBConfig.ConnectionString,//必填, 数据库连接字符串
                DbType = (SqlSugar.DbType)DBConfig.DbType,//必填, 数据库类型
                IsAutoCloseConnection = true,//默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                InitKeyType = InitKeyType.SystemTable //默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等信息
            }));
            services.AddAutoMapperSetup();
            var jwtSetting = ServerJwtSetting.GetJwtSetting();

            // 角色与接口的权限要求参数
            var permissionRequirement = new PermissionRequirement(
                "/api/error",// 拒绝授权的跳转地址（目前无用）
                new List<PermissionItem>(),
                ClaimTypes.Role,//基于角色的授权
                jwtSetting.Issuer,//发行人
                jwtSetting.Audience,//听众
                jwtSetting.Credentials,//签名凭据
                expiration: TimeSpan.FromSeconds(60 * 60)//接口的过期时间
                );


            // 3、复杂的策略授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.Name,
                         policy => policy.Requirements.Add(permissionRequirement));
            });

            // 注入权限处理器
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);

            services.AddAuthentication(o => {
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = nameof(ApiResponseHandler);
                    o.DefaultForbidScheme = nameof(ApiResponseHandler);
                })
               .AddJwtBearer(options =>
               {
                   options.Events = new JwtBearerEvents()
                   {
                       OnAuthenticationFailed = context =>
                       {
                           // 如果过期，则把<是否过期>添加到，返回头信息中
                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                           {
                               context.Response.Headers.Add("Token-Expired", "true");
                           }
                           return Task.CompletedTask;
                       }
                   };

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidIssuer = jwtSetting.Issuer,
                       ValidAudience = jwtSetting.Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecurityKey)),
                       ClockSkew = TimeSpan.Zero
                   };
               })
               .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), o => { });

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("BlogVue", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Framework.Core API",
                    Description = "API for Framework.Core",
                });
                // 开启加权小锁
                option.OperationFilter<AddResponseHeadersFilter>();
                option.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header中添加token，传递到后台
                option.OperationFilter<SecurityRequirementsOperationFilter>();

                // 必须是 oauth2
                option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权在下框中输入 Bearer Token值（注意两者之间是一个空格）",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                //// 配置apixml名称
                option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml"), true);
            });
            services.AddControllers();
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            #region AOP

            var cacheType = new List<Type>();
            builder.RegisterType<FrameworkCacheAOP>();
            cacheType.Add(typeof(FrameworkCacheAOP));
            builder.RegisterType<FrameworkLogAOP>();
            cacheType.Add(typeof(FrameworkLogAOP));
            builder.RegisterType<FrameworkTranAOP>();
            cacheType.Add(typeof(FrameworkTranAOP));
            #endregion

            #region 注入Repository

            var repositoryDllFile = Path.Combine(basePath, "Framework.Core.Repository.dll");
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();

            #endregion

            #region 注入Services

            var ServicesDllFile = Path.Combine(basePath, "Framework.Core.Services.dll");
            var assemblysServices = Assembly.LoadFrom(ServicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                 .InstancePerDependency()
                .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                .InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/BlogVue/swagger.json", "Framework.Core");

                option.RoutePrefix = string.Empty;
                option.DocumentTitle = "Framework.Core API";
            });


            app.UseRouting();
            // 先开启认证
            app.UseAuthentication();
            // 然后是授权中间件
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
