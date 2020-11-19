using System;

namespace TestService.Models
{
    [Serializable]
    public class Answer
    {
        public int Id { get; private set; }
        public string Value { get; private set; }
        public int AnswerIndex { get; private set; }
        public int QuestionId { get; private set; }
        public Question Question { get; private set; }

        private Answer() { }

        public Answer(string value, int answerIndex ,int questionId)
        {
            Value = value;
            AnswerIndex = answerIndex;
            QuestionId = questionId;
        }
    }
}