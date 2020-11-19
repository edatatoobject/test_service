using Microsoft.EntityFrameworkCore;
using TestService.Models;

namespace TestService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestAnswer> TestAnswers { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Test>().ToTable("Tests", "Test");
            builder.Entity<TestAnswer>().ToTable("TestAnswers", "Test");
        }
    }
}