using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class Ticket
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public bool Paid { get; set; }
    public int ZoneId { get; set; }
    public int ParkingSpaceId { get; set; }
    public int Fine { get; set; } // in RSD
}
