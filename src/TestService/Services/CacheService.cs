using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using TestService.Services.Interfaces;

namespace TestService.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        public CacheService(IDistributedCache distributedCache)
        {
             _distributedCache = distributedCache;
        }

        public string Get(string key)
        {
            var result = _distributedCache.Get(key);
            if (result == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(result);
        }

        public T Get<T>(string key)
        {
            var result = _distributedCache.Get(key);
            if (result == null)
            {
                return default(T);
            }

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(result))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }

        public void Set(string key, string content)
        {

            _distributedCache.Set(key, Encoding.UTF8.GetBytes(content));
        }

        public void Set<T>(string key, T content)
        {

            _distributedCache.Set(key, GetBytes<T>(content));
        }

        public void Set<T>(string key, T content, TimeSpan offset)
        {
            var options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(offset);
            _distributedCache.Set(key, GetBytes<T>(content), options);
        }

        private byte[] GetBytes<T>(T obj)
        {

            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, obj);
            return mStream.ToArray();
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }
    }
}
