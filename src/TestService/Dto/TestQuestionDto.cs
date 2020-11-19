using System.Collections.Generic;

namespace TestService.Dto
{
    public class TestQuestionDto
    {
        public string Question { get; set; }
        
        public List<string> Answers { get; set; }

        public int AnswersCount { get; set; }
    }
}