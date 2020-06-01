using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MelcoreFramework.Cache.Redis.Concrete
{
    public class RedisCache : IMelcoreCache
    {
        private readonly IDistributedCache _distributedCache;
        public RedisCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;

        }
        public Task Add<T>(string key, T data, int timeOut, CancellationToken token = default(CancellationToken))
        {
            var bytes = ObjectToByteArray(data);
            if (bytes == null || bytes.Length == 0)
            {
                throw new InvalidOperationException();
            }
            return _distributedCache.SetAsync(key, bytes, token);
        }

        public Task<T> Get<T>(string key, CancellationToken token = default(CancellationToken))
        {
            return GetAsync<T>(key, token);
        
        }
        public async Task<T> GetAsync<T>(string key, CancellationToken token = default(CancellationToken))
        {
            var array = await _distributedCache.GetAsync(key, token);
            if (array == null || array.Length == 0) {
                throw new KeyNotFoundException(key);
            }
            return ByteArrayToObject<T>(array); 
        }

        public Task Remove(string key, CancellationToken token = default(CancellationToken))
        {
           return  _distributedCache.RemoveAsync(key, token);
        }

        byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
            {
                return new byte[] { };
            }
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        private T ByteArrayToObject<T>(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            T obj = (T)binForm.Deserialize(memStream);

            return obj;
        }
    }
}
