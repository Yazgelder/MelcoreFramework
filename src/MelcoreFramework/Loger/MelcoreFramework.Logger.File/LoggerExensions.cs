using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace MelcoreFramework.Logger.File
{
    public static class LoggerExensions
    {
        public static void AddFileLogger(string path, Dictionary<string, LogEventLevel> keyValues, RollingInterval interval = RollingInterval.Day, string template = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
        {
            var q = new LoggerConfiguration()
       .Enrich.FromLogContext()
       //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
       //.MinimumLevel.Override("System", LogEventLevel.Warning)
       ;
            foreach (var item in keyValues.Keys)
            {
                q = q.MinimumLevel.Override(item, keyValues[item]);
            }
            Log.Logger = q.WriteTo.File(path, rollingInterval: interval, outputTemplate: template)
            .CreateLogger();
            Log.Information("WebApi Starting...");
        }
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, Action<RequestLoggingOptions> configureOptions = null)
        {
            app.UseSerilogRequestLogging(configureOptions);
            return app;
        }
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, string messageTemplate)
        {
            app.UseSerilogRequestLogging(messageTemplate);
            return app;
        }
    }
}
