using HatcheryFinal_Web_API.Data.Entities;
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

        public DbSet<CreditRequest> CreditRequests { get; set; }
        public DbSet<CreditPartner> CreditPartners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(_config.GetConnectionString("BankDb"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<CreditRequest>()
                .Property(d => d.Credit)
                .HasPrecision(6, 2);

            modelBuilder.Entity<CreditPartner>()
            .HasMany(d => d.Requests)
            .WithOne(d => d.Partner)
            .HasForeignKey(d => d.Token)
            .HasPrincipalKey(d => d.Token);

        }
    }
}
