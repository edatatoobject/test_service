using System;
using System.Collections.Generic;
using System.Linq;

namespace TestService.Models
{
    public class Test
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public ICollection<TestAnswer> TestAnswers { get; private set; }

        public Test(string name, ICollection<TestAnswer> testAnswers = null)
        {
            Name = name;
            TestAnswers = testAnswers;
        }
        
        private Test(){}
        
        
    }
}