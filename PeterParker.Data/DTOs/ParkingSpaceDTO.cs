using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class ParkingSpaceDTO
    {
        public Guid GUID { get; set; }
        public VehicleDTO Vehicle { get; set; } = null;
        public int Number { get; set; }
    }
}
