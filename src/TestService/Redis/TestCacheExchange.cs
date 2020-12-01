using System;
using System.Collections.Generic;
using TestService.Dto;
using TestService.Models;
using TestService.Services.Interfaces;

namespace TestService.Redis
{
    public class TestCacheExchange
    {
        private readonly ICacheService _cacheService;

        public TestCacheExchange(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public void SetSessioCache(string sessionId, TestInitializeResponse testInitializeResponse)
        {
            _cacheService.Set(sessionId, testInitializeResponse, TimeSpan.FromHours(1));
        }
        
        public void RemoveSessionCache(string sessionId)
        {
            _cacheService.Remove(sessionId);
        }

        public void SetAnswerInCache(string sessionId, int questionIndex, int answerIndex)
        {
            _cacheService.Set($"{sessionId}{questionIndex.ToString()}", answerIndex, TimeSpan.FromHours(1));
        }

        public List<int> GetAnswersFromCache(string sessionId, int questionCount)
        {
            var answers = new List<int>();

            for (int questionIndex = 0; questionIndex < questionCount; questionIndex++)
            {
                string key = $"{sessionId}{questionIndex.ToString()}";
                int answerIndex = _cacheService.Get<int>(key);
                answers.Add(answerIndex);
                _cacheService.Remove(key);
            }

            return answers;
        }

        public void RemoveAnswersFromCache(string sessionId, int questionCount)
        {
            for (int answerIndex = 0; answerIndex < questionCount; answerIndex++)
            {
                _cacheService.Remove($"{sessionId}{answerIndex}");
            }
        }
    }
}