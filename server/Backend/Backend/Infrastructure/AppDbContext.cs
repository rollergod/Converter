using Backend.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //datetime kind - local
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyConverter> CurrencyConverters { get; set; }
        public DbSet<Transfer> Transfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasData(
                new Currency{ Id = 1, Name = "RUB" },
                new Currency { Id = 2, Name = "USD" }
            );

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.FromAccount)
                .WithMany(x => x.TransfersFrom)
                .HasForeignKey(x => x.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.ToAccount)
                .WithMany(x => x.TransfersTo)
                .HasForeignKey(x => x.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
