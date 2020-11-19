using System;

namespace TestService.Services.Interfaces
{
    public interface ICacheService
    {
        string Get(string key);
        T Get<T>(string key);
        void Set(string key, string content);
        void Set<T>(string key, T content);
        void Set<T>(string key, T content, TimeSpan offset);
        void Remove(string key);
    }
}