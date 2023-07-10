using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class ParkingSpaceDTO
    {
        public string Street { get; set; } = string.Empty;
        public string VehicleRegistration { get; set; } = string.Empty;
        public string GarageName { get; set; } = string.Empty;
        public int Number { get; set; }
    }
}
