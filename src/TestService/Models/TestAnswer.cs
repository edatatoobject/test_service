using System;
using System.Collections.Generic;

namespace TestService.Models
{
    public class TestAnswer
    {
        public int Id { get; set; }
        
        public string Value { get; private set; }
        public int QuestionIndex { get; private set; }
        public int TestId { get; private set; }
        public Test Test { get; set; }

        private TestAnswer() { }

        public TestAnswer(string value, int questionIndex, int testId)
        {
            QuestionIndex = questionIndex;
            TestId = testId;
        }
    }
}