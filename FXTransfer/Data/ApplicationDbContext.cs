using FXTransfer.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FXTransfer.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<FeeConfiguration> FeeConfigurations { get; set; }
    public DbSet<AdminActionLog> AdminActionLogs { get; set; }
    public DbSet<RateAlert> RateAlerts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Wallet>()
            .HasIndex(w => new { w.UserId, w.CurrencyCode })
            .IsUnique();

        modelBuilder.Entity<Transfer>()
            .HasIndex(t => t.UserId);

        modelBuilder.Entity<Transfer>()
            .HasIndex(t => t.Status);

        modelBuilder.Entity<Transfer>()
            .HasIndex(t => t.CreatedAt);
    }
}