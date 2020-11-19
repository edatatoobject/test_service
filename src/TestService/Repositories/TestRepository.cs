using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestService.Data;
using TestService.Models;

namespace TestService.Repositories
{
    public class TestRepository
    {
        private readonly ApplicationDbContext _context;

        public TestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Test GetTestWithQuestions(int testId)
        {
            return _context.Tests.Include(t => t.Questions).ThenInclude(q => q.Answers)
                .FirstOrDefault(t => t.Id == testId);
        }

        public List<Test> GetRange()
        {
            return _context.Tests.ToList();
        }
    }
}