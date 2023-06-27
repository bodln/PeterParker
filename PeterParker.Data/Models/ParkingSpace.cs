using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class ParkingSpace
{
    public int Id { get; set; }
    public bool Status { get; set; } = false; // false = empty
    public string Street { get; set; } = string.Empty;
    public string VehicleRegistration { get; set; } = string.Empty;
    public int ZoneId { get; set; }
    public int Number { get; set; } // in zone
}
