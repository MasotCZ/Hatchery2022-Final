using HatcheryFinal_Web_API.Data.Dao;
using Microsoft.EntityFrameworkCore;

namespace HatcheryFinal_Web_API.Data
{
    class BankDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public BankDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<CreditRequestDao> CreditRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(_config.GetConnectionString("BankDb"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<CreditRequestDao>()
                .Property(d => d.Credit)
                .HasPrecision(6, 2);
        }
    }
}
