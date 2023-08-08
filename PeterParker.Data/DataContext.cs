using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PeterParker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data;

public class DataContext : IdentityDbContext<User>
{
    public DataContext() : base()
    {

    }
    public DataContext(DbContextOptions<DataContext> option) : base (option)
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
    public DbSet<Garage> Garage { get; set; }
    public DbSet<Pass> Passes { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    //public DbSet<UserRefreshToken> RefreshTokens { get; set; }
}
