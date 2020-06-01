using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MelcoreFramework.Logger.ElasticSearch;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MelcoreFramework.Presentation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggerExensions.AddElasticSearchLogger(
                Assembly.GetExecutingAssembly().FullName.Replace(".","-"),
                new Dictionary<string, Serilog.Events.LogEventLevel>(),
                new Uri("http://elastic:*******************@elastiksearch.software.moda"),
                "test-template-"+DateTime.Now.ToString("yyyy-MM"),
                "test-template-" + DateTime.Now.ToString("yyyy-MM")
                );
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureLogging((hostingContext, config) => { config.ClearProviders(); })
            .UseSerilog();
    }
}
