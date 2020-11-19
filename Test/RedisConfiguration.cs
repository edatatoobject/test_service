using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Test
{
    public static class RedisConfiguration
    {
        private static String Host = "127.0.0.1:6379";
        private static String Instance = "test";
        
        public static IDistributedCache GetRedisConfiguration()
        {

            var redisOptions = new RedisCacheOptions
            {
                ConfigurationOptions = new ConfigurationOptions()
            };
            redisOptions.ConfigurationOptions.EndPoints.Add(Host);
            redisOptions.InstanceName = Instance;
            var opts = Options.Create(redisOptions);
            
            IDistributedCache cache = new RedisCache(opts);
            return cache;
        }
    }
}