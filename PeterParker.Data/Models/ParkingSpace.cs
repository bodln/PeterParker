using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class ParkingSpace
{
    public int Id { get; set; }
    public Guid GUID { get; set; }
    public Vehicle? Vehicle { get; set; }
    public int Number { get; set; } // in zone
}
