using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class ParkVehicleDTO
    {
        public Guid VehicleGuid { get; set; }
        public Guid ParkingSpaceGuid { get; set; }
    }
}
