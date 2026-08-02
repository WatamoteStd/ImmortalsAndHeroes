using MasterServer.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace MasterServer.Data;

public class AppDbContext : DbContext
{

    
    public AppDbContext(DbContextOptions<AppDbContext> option) : base (option)
    {

    }

    public DbSet<User> Users {get; set;}
    public DbSet<Character> Characters {get; set;}



}