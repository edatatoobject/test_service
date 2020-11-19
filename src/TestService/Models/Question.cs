using System;
using System.Collections.Generic;

namespace TestService.Models
{
    public class Question
    {
        public int Id { get; set; }
        
        public string Value { get; private set; }
        public int QuestionIndex { get; private set; }
        public int CorrectAnswerIndex { get; private set; }
        public int TestId { get; private set; }
        public Test Test { get; set; }

        public ICollection<Answer> Answers;

        private Question() { }

        public Question(string value, int questionIndex, int testId, ICollection<Answer> answers = null)
        {
            Value = value;
            QuestionIndex = questionIndex;
            TestId = testId;
            Answers = answers;
        }

        public int GetAnswersCount()
        {
            return Answers.Count;
        }

        public bool IsAnswerIndexExists(int answerIndex)
        {
            return Answers.Count > answerIndex;
        }

        public bool IsCorrectAnswer(int answerIndex)
        {
            return CorrectAnswerIndex == answerIndex;
        }
    }
}