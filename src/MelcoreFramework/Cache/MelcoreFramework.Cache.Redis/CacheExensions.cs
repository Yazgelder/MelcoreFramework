using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MelcoreFramework.Cache.Redis
{
    public static class CacheExensions
    {

        public static IServiceCollection AddMelcoreRedisCache(this IServiceCollection services, string configuration, string instanceName, string password, int? defaultDatabase = null)
        {
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = configuration;
                options.InstanceName = instanceName;
                options.ConfigurationOptions.Password = password;
                options.ConfigurationOptions.DefaultDatabase = defaultDatabase;
            });
            services.AddMemoryCache();
            return services;
        }

    }
}
