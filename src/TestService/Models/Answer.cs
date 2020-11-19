namespace TestService.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        private Answer() { }

        public Answer(string value, int questionId)
        {
            Value = value;
            QuestionId = questionId;
        }
    }
}