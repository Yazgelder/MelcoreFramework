using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MelcoreFramework.Cache.Memory
{
  public  static class CacheExensions
    {
        public static IServiceCollection AddMelcoreMemoryCache(this IServiceCollection services, string configuration, string instanceName, string password, int? defaultDatabase = null)
        {
            services.AddDistributedMemoryCache();
            services.AddMemoryCache();
            return services;
        }
    }
}
