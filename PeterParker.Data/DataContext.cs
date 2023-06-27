using Microsoft.EntityFrameworkCore;
using PeterParker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base (options)
    {

    }

    public DbSet<Zone> Zones { get; set; }
    public DbSet<ParkingSpace> ParkingSpaces { get; set; }
    public DbSet<Garage> Garage { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Pass> Passes { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Inspector> Inspectors { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
}
