using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore;
using TestService.Models;

namespace TestService.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var test = GetPreconfiguredTest();
            
            if (!(await context.Tests.AnyAsync()))
            {
                await context.Tests.AddAsync(test);
            }
        }

        private static Test GetPreconfiguredTest()
        {
            return new Test(1, "Test", new List<Question>
            {
                new Question(1, "Test", 0, 1, new List<Answer>
                {
                    new Answer("Answer",0, 1),
                    new Answer("Answer",0, 1),
                    new Answer("Answer",0, 1),
                }),
                new Question(2, "Test", 1, 1, new List<Answer>
                {
                    new Answer("Answer",0, 2),
                    new Answer("Answer",0, 2),
                    new Answer("Answer",0, 2),
                }),
                new Question(3, "Test", 2, 1, new List<Answer>
                {
                    new Answer("Answer",0, 3),
                    new Answer("Answer",0, 3),
                    new Answer("Answer",0, 3),
                }),
                new Question(4, "Test", 3, 1, new List<Answer>
                {
                    new Answer("Answer",0, 4),
                    new Answer("Answer",0, 4),
                    new Answer("Answer",0, 4),
                }),
            });
        }
    }
}