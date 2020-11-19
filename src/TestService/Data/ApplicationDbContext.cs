using Microsoft.EntityFrameworkCore;
using TestService.Models;

namespace TestService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Test>().ToTable("Tests", "Test");
            builder.Entity<Question>().ToTable("Questions", "Test");
            builder.Entity<Answer>().ToTable("Answers", "Test");
        }
    }
}