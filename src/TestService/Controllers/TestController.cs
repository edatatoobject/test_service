using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Distributed;
using TestService.Data;
using TestService.Dto;
using TestService.Models;
using TestService.Redis;
using TestService.Repositories;
using TestService.Services.Interfaces;

namespace TestService.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly TestCacheExchange _testCacheExchange;
        private readonly TestRepository _testRepository;

        public TestController(ICacheService cacheService, ApplicationDbContext context)
        {
            _testCacheExchange = new TestCacheExchange(cacheService);
            _testRepository = new TestRepository(context);
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<Test>> GetAllTests()
        {
            return _testRepository.GetRange();
        }

        [HttpGet]
        [Route("Initialize")]
        public ActionResult<TestInitializeResponse> InitializeTest(string userName, int testId)
        {
            var sessionId = Guid.NewGuid().ToString();

            //try get test from cache
            var test = GetTest(testId);
            if (test == null)
            {
                return BadRequest("Test not found");
            }

            var testInitializeResponse = new TestInitializeResponse
            {
                SessionId = sessionId, TestName = test.Name, UserName = userName,
                NumberOfQuestion = test.GetNumberOfQuestions()
            };

            _testCacheExchange.SetSessioCache(sessionId, testInitializeResponse);

            return new TestInitializeResponse
                {SessionId = sessionId, TestName = test.Name, NumberOfQuestion = test.GetNumberOfQuestions()};
        }

        [HttpGet]
        [Route("Question")]
        public ActionResult<TestQuestionDto> GetQuestion(int testId, int questionIndex)
        {
            var test = GetTest(testId);
            if (test == null)
            {
                return BadRequest("Test not found");
            }

            var question = test.GetQuestion(questionIndex);

            return new TestQuestionDto
            {
                Question = question.Value, Answers = question.Answers.Select(a => a.Value).ToList(),
                AnswersCount = question.Answers.Count
            };
        }

        [HttpPost]
        [Route("Answer")]
        public ActionResult SendAnswer(string sessionId, int testId, int questionIndex, int answerIndex)
        {
            //found test
            var test = GetTest(testId);
            if (test == null)
            {
                return BadRequest("Test not found");
            }

            var question = test.GetQuestion(questionIndex);

            if (!question.IsAnswerIndexExists(answerIndex))
            {
                return BadRequest("Answer index not found");
            }

            _testCacheExchange.SetAnswerInCache(sessionId, questionIndex, answerIndex);

            return Ok();
        }

        [HttpGet]
        [Route("Complete")]
        public ActionResult<TestCompleteDto> CompleteTest(string sessionId, int testId)
        {
            var test = GetTest(testId);

            int questionsCount = test.GetNumberOfQuestions();

            var answers = _testCacheExchange.GetAnswersFromCache(sessionId, questionsCount);

            var correctAnswers = 0;
            for (int questionIndex = 0; questionIndex < test.GetNumberOfQuestions(); questionIndex++)
            {
                var question = test.GetQuestion(questionIndex);
                if (question.IsCorrectAnswer(answers[questionIndex]))
                {
                    correctAnswers += 1;
                }
            }

            _testCacheExchange.RemoveSessionCache(sessionId);

            return new TestCompleteDto {QuestionsCount = questionsCount, CorrectAnswers = correctAnswers};
        }

        [HttpGet]
        [Route("Cancel")]
        public ActionResult CancelTest(string sessionId, int testId)
        {
            var test = GetTest(testId);
            _testCacheExchange.RemoveAnswersFromCache(sessionId, test.GetNumberOfQuestions());
            _testCacheExchange.RemoveSessionCache(sessionId);
            return Ok();
        }

        private Test GetTest(int testId)
        {
            //get test from database and save in cache
            var test = _testRepository.GetTestWithQuestions(testId);

            return test;
        }
    }
}