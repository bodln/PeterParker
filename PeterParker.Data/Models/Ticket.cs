using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class Ticket
{
    public int Id { get; set; }
    // The offending Vehicle can be derived from the ParkingSpace (TableOptimisationV1)
    //public Vehicle Vehicle { get; set; } 
    public bool Paid { get; set; }
    public Zone Zone { get; set; }
    public ParkingSpace ParkingSpace { get; set; }
    public int Fine { get; set; } // in RSD
}
