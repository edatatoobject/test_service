using Microsoft.EntityFrameworkCore;
using TestService.Data;

namespace Test
{
    public static class DbContextConfiguration
    {
        private const string ConnectionString =
            "Server=192.168.0.2;Port=5433;Database=testService;User Id=postgres;Password=123;";
        
        public static ApplicationDbContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(ConnectionString);

            ApplicationDbContext dbContext = new ApplicationDbContext(optionsBuilder.Options);

            ApplicationDbContextSeed.SeedAsync(dbContext);

            return dbContext;
        }
    }
}