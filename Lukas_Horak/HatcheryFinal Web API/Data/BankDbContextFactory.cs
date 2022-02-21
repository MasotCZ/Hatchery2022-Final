using Microsoft.EntityFrameworkCore;

namespace HatcheryFinal_Web_API.Data
{
    class BankDbContextFactory : IDbContextFactory<BankDbContext>
    {
        public BankDbContext CreateDbContext()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return new BankDbContext(new DbContextOptionsBuilder<BankDbContext>().Options, config);
        }
    }
}
