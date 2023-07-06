using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class Garage
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Type { get; set; } = string.Empty;
    [Required]
    public string Address { get; set; } = string.Empty;
    // These are my thoughts: 
    // The Garage class should keep the Zone it belongs to only for the sake of knowing where it belongs.
    // When the Inspector or User check the ParkingSpaces in a zone for whatever purpose they can find out which Garage
    // that ParkingSpace belongs to if it belongs to any at all. Also the TotalSpaces can be derived from the ParkinSpace
    // list in the Zone that the Garage belongs to and on top of that when a ParkingSpace is occupied/vacated the 
    // FreeSpaces count can be easily updated. (TableOptimisationV1)
    [Required]
    public Zone Zone { get; set; }
    public int TotalSpaces { get; set; }
    public int FreeSpaces { get; set; }
    [Required]
    public string WorkingHours { get; set; } = string.Empty; // ???
}
