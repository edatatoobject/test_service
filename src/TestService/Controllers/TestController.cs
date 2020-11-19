using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Distributed;
using TestService.Data;
using TestService.Dto;
using TestService.Models;
using TestService.Repositories;
using TestService.Services.Interfaces;

namespace TestService.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly TestRepository _testRepository;

        public TestController(ICacheService cacheService, ApplicationDbContext context)
        {
            _cacheService = cacheService;
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

            var testInitializeResponseDto = new TestInitializeResponse
            {
                SessionId = sessionId, TestName = test.Name, UserName = userName,
                NumberOfQuestion = test.GetNumberOfQuestions()
            };

            _cacheService.Set(sessionId, testInitializeResponseDto, TimeSpan.FromHours(1));

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

            SetAnswerInCache(sessionId, questionIndex, answerIndex);

            return Ok();
        }

        [HttpGet]
        [Route("Complete")]
        public ActionResult<TestCompleteDto> CompleteTest(string sessionId, int testId)
        {
            var test = GetTest(testId);

            int questionsCount = test.GetNumberOfQuestions();

            var answers = GetAnswersFromCache(sessionId, questionsCount);

            var correctAnswers = 0;
            for (int questionIndex = 0; questionIndex < test.GetNumberOfQuestions(); questionIndex++)
            {
                var question = test.GetQuestion(questionIndex);
                if (question.IsCorrectAnswer(answers[questionIndex]))
                {
                    correctAnswers += 1;
                }
            }

            RemoveSessionCache(sessionId);

            return new TestCompleteDto {QuestionsCount = questionsCount, CorrectAnswers = correctAnswers};
        }

        [HttpGet]
        [Route("Cancel")]
        public ActionResult CancelTest(string sessionId, int testId)
        {
            var test = GetTest(testId);
            RemoveAnswersFromCache(sessionId, test.GetNumberOfQuestions());
            RemoveSessionCache(sessionId);
            return Ok();
        }

        private Test GetTest(int testId)
        {
            //get test from database and save in cache
            var test = _testRepository.GetTestWithQuestions(testId);

            return test;
        }

        private void RemoveSessionCache(string sessionId)
        {
            _cacheService.Remove(sessionId);
        }

        private Test GetTestFromCache(int testId)
        {
            return _cacheService.Get<Test>($"test{testId.ToString()}");
        }

        private void SetTestInCache(Test test)
        {
            _cacheService.Set($"test{test.Id.ToString()}", test);
        }

        private void SetAnswerInCache(string sessionId, int questionIndex, int answerIndex)
        {
            _cacheService.Set($"{sessionId}{questionIndex.ToString()}", answerIndex, TimeSpan.FromHours(1));
        }

        private List<int> GetAnswersFromCache(string sessionId, int questionCount)
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

        private void RemoveAnswersFromCache(string sessionId, int questionCount)
        {
            for (int answerIndex = 0; answerIndex < questionCount; answerIndex++)
            {
                _cacheService.Remove($"{sessionId}{answerIndex}");
            }
        }
    }
}