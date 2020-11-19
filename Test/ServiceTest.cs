using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using TestService.Controllers;
using TestService.Data;
using TestService.Dto;
using TestService.Services;
using TestService.Services.Interfaces;
using Xunit;

namespace Test
{
    public class ServiceTest
    {
        private const string UserName = "xUnit";

        private readonly ICacheService _cacheService;
        private readonly ApplicationDbContext _context;

        private readonly TestController _testController;

        public ServiceTest()
        {
            _cacheService = new CacheService(RedisConfiguration.GetRedisConfiguration());
            _context = DbContextConfiguration.GetDbContext();
            _testController = new TestController(_cacheService, _context);
        }


        [Fact]
        public async Task SuccessfulTest()
        {

            var currentTest = GetFirstTest();
            
            var initializeTestDto = _testController.InitializeTest(UserName, currentTest.Id).Value;

            for (int questionIndex = 0; questionIndex < initializeTestDto.NumberOfQuestion; questionIndex++)
            {
                var question = _testController.GetQuestion(currentTest.Id, questionIndex);

                var answerResponse = _testController.SendAnswer(initializeTestDto.SessionId, currentTest.Id, questionIndex,
                    new Random().Next(0, question.Value.AnswersCount));
            }

            var complete = _testController.CompleteTest(initializeTestDto.SessionId, currentTest.Id);

            var cachedUser = _cacheService.Get<TestInitializeResponse>(initializeTestDto.SessionId);

            if (cachedUser != null)
            {
                throw new ArgumentException("Redis was not flushed");
            }
        }

        [Fact]
        public async Task CancelTask()
        {
            var currentTest = GetFirstTest();
            var initializeTestDto = _testController.InitializeTest(UserName, currentTest.Id).Value;

            _testController.CancelTest(initializeTestDto.SessionId, currentTest.Id);
            
            var cachedUser = _cacheService.Get<string>(initializeTestDto.SessionId);

            if (cachedUser != null)
            {
                throw new ArgumentException("Redis was not flushed");
            }
        }

        private TestService.Models.Test GetFirstTest()
        {
            var tests = _testController.GetAllTests().Value;
            if (tests == null || !tests.Any())
            {
                throw new NullReferenceException("Test list is empty");
            }

            return tests.FirstOrDefault();
        }
    }
}