using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class Ticket
{
    public int Id { get; set; }
    public Guid GUID { get; set; }
    public bool Paid { get; set; } = false;
    public Guid ZoneGuid { get; set; }
    public Guid ParkingSpaceGuid { get; set; }
    public int Fine { get; set; } // in RSD
    public string Reason { get; set; }
}
