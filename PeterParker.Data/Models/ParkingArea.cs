using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;
public class ParkingArea
{
    public int Id { get; set; }
    public Guid GUID { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Type { get; set; } = string.Empty;
    [Required]
    public string Address { get; set; } = string.Empty;
    public string GeoJSON { get; set; } = string.Empty;
    public string WorkingHours { get; set; } = string.Empty; // ???
    public List<ParkingSpace> ParkingSpaces { get; set; } = new List<ParkingSpace>();
}
