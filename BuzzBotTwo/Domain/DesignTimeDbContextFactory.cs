using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BuzzBotTwo.Domain
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BotContext>
    {
        public BotContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder()
                .UseSqlite("DataSource=BuzzBotDataTwo.db")
                .EnableSensitiveDataLogging();
            return new BotContext(optionsBuilder.Options);
        }
    }
}