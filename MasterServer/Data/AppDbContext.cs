using MasterServer.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace MasterServer.Data;

public class AppDbContext : DbContext
{

    
    public AppDbContext(DbContextOptions<AppDbContext> option) : base (option)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MarketOrders>(entity =>
        {
            entity.HasIndex(m => new {m.LocationId, m.IsSold, m.ItemType});
        });
    }

    public DbSet<User> Users {get; set;}
    public DbSet<Character> Characters {get; set;}
    public DbSet<MarketOrders> GlobalMarket {get; set;}



}