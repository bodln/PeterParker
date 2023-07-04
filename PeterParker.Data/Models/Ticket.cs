using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class Ticket
{
    public int Id { get; set; }
    public string VehicleRegistration { get; set; } = string.Empty;
    public bool Paid { get; set; }
    public int ZoneId { get; set; }
    public int ParkingSpaceId { get; set; }
    public int Fine { get; set; }
}
