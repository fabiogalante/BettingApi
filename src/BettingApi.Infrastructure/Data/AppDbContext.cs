using BettingApi.Core.Bets;
using BettingApi.Core.Common;
using BettingApi.Core.Customers;
using Microsoft.EntityFrameworkCore;

namespace BettingApi.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Bet> Bets { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasConversion(
                id => id.Value,
                value => EntityId.From(value));
        
            entity.HasMany(c => c.Bets)
                .WithOne(b => b.Customer)
                .HasForeignKey(b => b.CustomerId);
        });
    
        modelBuilder.Entity<Bet>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Id).HasConversion(
                id => id.Value,
                value => EntityId.From(value));
            
            entity.Property(b => b.CustomerId).HasConversion(
                id => id.Value,
                value => EntityId.From(value));
        });
    }
}