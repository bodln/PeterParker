using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class Garage
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public Zone Zone { get; set; }
    public int TotalSpaces { get; set; }
    public int FreeSpaces { get; set; }
    public string WorkingHours { get; set; } = string.Empty; // ???
}
