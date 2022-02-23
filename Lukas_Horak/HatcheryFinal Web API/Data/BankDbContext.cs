using HatcheryFinal_Web_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;

namespace HatcheryFinal_Web_API.Data
{
    public interface IBankDbContext
    {
        DbSet<CreditRequest> CreditRequests { get; set; }
        DbSet<CreditPartner> CreditPartners { get; set; }

        EntityEntry Add(object entity);
        EntityEntry Remove(object entity);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    internal class BankDbContext : DbContext, IBankDbContext
    {
        private readonly IConfiguration _config;

        public BankDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<CreditRequest> CreditRequests { get; set; }
        public DbSet<CreditPartner> CreditPartners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            optionsBuilder.UseSqlServer(_config.GetConnectionString("BankDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<CreditRequest>()
                .Property(d => d.Credit)
                .HasPrecision(8, 2);

            modelBuilder.Entity<CreditPartner>()
            .HasMany(d => d.Requests)
            .WithOne(d => d.Partner)
            .HasForeignKey(d => d.Token)
            .HasPrincipalKey(d => d.Token);

        }
    }
}
