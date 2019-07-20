using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TouchNetCore.Auth;
using TouchNetCore.Auth.Authentication;
using TouchNetCore.Auth.Security.Session;
using TouchNetCore.Business.Infrastructure.Repository;
using TouchNetCore.Component.Autofac;
using TouchNetCore.Component.Redis;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using TouchNetCore.WebApi.MiddleWare.ExceptionHandle;
using log4net.Repository;
using log4net;
using log4net.Config;
using TouchNetCore.WebApi.MiddleWare.Log;

namespace TouchNetCore.WebApi
{
    public class Startup
    {
        private static readonly string _swaggerDocName = "v1";
        private readonly IHostingEnvironment _hostingEnvironment;
        public static ILoggerRepository Repository { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _hostingEnvironment = env;
            Repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(Repository, new FileInfo("log4net.config"));
            InitRepository.LoggerRepository = Repository;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(o=>{
                o.Filters.Add(typeof(GlobalExceptions));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            //注入sql上下文实例
            services.AddDbContext<TouchDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("TouchConnection")));

            //添加身份验证
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = BearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = BearerDefaults.AuthenticationScheme;
            }).AddBearer();

            services.AddSwaggerGen(c =>
            {
                c.IgnoreObsoleteActions();
                c.SwaggerDoc(
                    // name: 攸關 SwaggerDocument 的 URL 位置。
                    name: _swaggerDocName,
                    // info: 是用於 SwaggerDocument 版本資訊的顯示(內容非必填)。
                    info: new Info
                    {
                        Title = "TouchNetCore",
                        Version = "1.0.0",
                        Description = "自定义权限验证示例",
                        //TermsOfService = "None",
                        //Contact = new Contact { Name = "Jayson Xu", Url = "wiseant@163.com" }
                    }
                );
                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } } };
                c.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "权限认证(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });//Authorization的设置
                //应用XML注释文档
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "TouchNetCore.WebApi.xml");
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
                c.IncludeXmlComments(@"E:\TouchNetCore\03 TouchNetCore.Business\TouchNetCore.Business\bin\Debug\netcoreapp2.2\TouchNetCore.Business.xml");
                c.IncludeXmlComments(@"E:\TouchNetCore\01 TouchNetCore.Component\TouchNetCore.Component.Utils\bin\Debug\netcoreapp2.2\TouchNetCore.Component.Utils.xml");
            });

            //ioc容器初始化
            return IocManager.Instance.Initialize(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Functest"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseMyExceptionHandler(loggerFactory);
            app.UseHttpsRedirection();
            app.UseMvcWithDefaultRoute();
            app.UseStaticFiles();

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    // url: 需配合 SwaggerDoc 的 name。 "/swagger/{SwaggerDoc name}/swagger.json"
                    url: $"../swagger/{_swaggerDocName}/swagger.json", //这里一定要使用相对路径，不然网站发布到子目录时将报告："Not Found /swagger/v1/swagger.json"
                                                                      // description: 用於 Swagger UI 右上角選擇不同版本的 SwaggerDocument 顯示名稱使用。
                    name: "RESTful API v1.0.0"
                );
                //c.InjectOnCompleteJavaScript();
            });
        }
    }
}
