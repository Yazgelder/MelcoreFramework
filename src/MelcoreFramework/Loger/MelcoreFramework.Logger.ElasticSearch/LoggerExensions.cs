using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;

namespace MelcoreFramework.Logger.ElasticSearch
{
    public static class LoggerExensions
    {
        #region Public Methods

        public static void AddElasticSearchLogger(string application, Dictionary<string, LogEventLevel> keyValues, Uri elasticsearchUrl, string templateName, string indexFormat)
        {
            var q = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", application)
                //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                //.MinimumLevel.Override("System", LogEventLevel.Warning)
                ;
            foreach (var item in keyValues.Keys)
            {
                q = q.MinimumLevel.Override(item, keyValues[item]);
            }

            Log.Logger = q.WriteTo.Elasticsearch(
                  new ElasticsearchSinkOptions(elasticsearchUrl)
                  {
                      CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                      AutoRegisterTemplate = true,
                      TemplateName = templateName,
                      IndexFormat = indexFormat,
                  })
              .MinimumLevel.Verbose()
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
        #endregion Public Methods
    }
}