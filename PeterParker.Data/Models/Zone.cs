using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class Zone
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public Guid GUID { get; set; }
    public string GeoJSON { get; set; } = string.Empty;
    public List<ParkingArea> ParkingAreas { get; set; } = new List<ParkingArea>();
}