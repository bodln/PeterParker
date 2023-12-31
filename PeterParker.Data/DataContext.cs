﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeterParker.Data.Models;

namespace PeterParker.Data;

public class DataContext : IdentityDbContext<User>
{
    public DataContext() : base()
    {

    }
    public DataContext(DbContextOptions<DataContext> option) : base(option)
    {

    }
    //This must be removed and a different solution implemented
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("server=.;database=PeterParkerDB;trusted_connection=true;");
        }
    }


    public DbSet<Zone> Zones { get; set; }
    public DbSet<ParkingSpace> ParkingSpaces { get; set; }
    public DbSet<ParkingArea> ParkingAreas { get; set; }
    public DbSet<Pass> Passes { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
