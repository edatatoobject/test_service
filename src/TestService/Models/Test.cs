using System;
using System.Collections.Generic;
using System.Linq;

namespace TestService.Models
{
    [Serializable]
    public class Test
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public ICollection<Question> Questions { get; private set; }

        public Test(int id, string name, ICollection<Question> questions = null)
        {
            Id = id;
            Name = name;
            Questions = questions;
        }

        private Test()
        {
        }

        public int GetNumberOfQuestions()
        {
            return Questions.GroupBy(t => t.QuestionIndex).Count();
        }

        public Question GetQuestion(int questionIndex)
        {
            var question = Questions.FirstOrDefault(q => q.QuestionIndex == questionIndex);

            return question;
        }
    }
}