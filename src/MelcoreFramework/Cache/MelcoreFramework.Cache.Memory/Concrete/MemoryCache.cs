using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MelcoreFramework.Cache.Memory.Concrete
{
    public class MemoryCache : IMelcoreCache
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task Add<T>(string key, T data, int timeOut, CancellationToken token = default(CancellationToken))
        {
            _memoryCache.Set<T>(key, data, TimeSpan.FromMinutes(timeOut));
            return Task.CompletedTask;
        }

     
        public Task<T> Get<T>(string key, CancellationToken token = default(CancellationToken))
        {
            return Task.FromResult(_memoryCache.Get<T>(key));
        }

        public Task Remove(string key, CancellationToken token = default(CancellationToken))
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }


    }
}
