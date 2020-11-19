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
            await context.Database.MigrateAsync();
            
            if (!(await context.Tests.AnyAsync()))
            {
                await context.Tests.AddAsync(GetPreconfiguredTest());
                await context.Questions.AddRangeAsync(GetPreconfiguredQuestions());
                await context.Answers.AddRangeAsync(GetPreconfiguredAnswers());
                await context.SaveChangesAsync();
            }
            
        }

        private static Test GetPreconfiguredTest()
        {
            return new Test(1, "Test");
        }

        private static List<Question> GetPreconfiguredQuestions()
        {
            return new List<Question>
            {
                
                new Question(1, "Test", 0, 1),
                new Question(2, "Test", 1, 1),
                new Question(3, "Test", 2, 1),
                new Question(4, "Test", 3, 1),
            };
        }

        private static List<Answer> GetPreconfiguredAnswers()
        {
            return new List<Answer>
            {
                new Answer("Answer", 0, 1),
                new Answer("Answer", 1, 1),
                new Answer("Answer", 2, 1),
                
                new Answer("Answer", 0, 2),
                new Answer("Answer", 1, 2),
                new Answer("Answer", 2, 2),
                
                new Answer("Answer", 0, 3),
                new Answer("Answer", 1, 3),
                new Answer("Answer", 2, 3),
                
                new Answer("Answer", 0, 4),
                new Answer("Answer", 1, 4),
                new Answer("Answer", 2, 4),
            };
        }
    }
}