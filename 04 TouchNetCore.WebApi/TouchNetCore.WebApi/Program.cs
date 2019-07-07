using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TouchNetCore.Component.Redis;

namespace TouchNetCore.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hosttingContext, config) =>
                {
                    var env = hosttingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", true, true).
                           AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                    IConfigurationRoot configuration = config.Build();
                    //初始化redis连接地址
                    RedisHelper.ConnectionString = configuration["RedisConnectionString"].ToString();
                })
                .UseStartup<Startup>();
    }
}
