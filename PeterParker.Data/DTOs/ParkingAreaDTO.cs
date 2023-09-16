using PeterParker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class ParkingAreaDTO
    {
        public string Name { get; set; } = string.Empty;
        public Guid GUID { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string GeoJSON { get; set; } = string.Empty;
        public string WorkingHours { get; set; } = string.Empty;
        public List<ParkingSpaceDTO> ParkingSpaces { get; set; } = new List<ParkingSpaceDTO>();
    }
}
