using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class Zone
{
    public int Id { get; set; }
    public string GeoJSON { get; set; } = string.Empty;
    public int TotalSpaces { get; set; }
    public int FreeSpaces { get; set; }
    public List<Garage> Garages { get; set; } = new List<Garage>();
    public List<ParkingSpace> ParkingSpaces { get; set; } = new List<ParkingSpace>(); // outside of garages
}
