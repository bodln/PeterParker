using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class VehicleDTO
    {
        public string Registration { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public Guid ParkingSpaceGuid { get; set; } = Guid.Empty;
        public Guid GUID { get; set; }
    }
}
