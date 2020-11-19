using System;

namespace TestService.Dto
{
    [Serializable]
    public class TestInitializeResponse
    {
        public string SessionId { get; set; }
        public string UserName { get; set; }
        public string TestName { get; set; }
        public int NumberOfQuestion { get; set; }
    }
}