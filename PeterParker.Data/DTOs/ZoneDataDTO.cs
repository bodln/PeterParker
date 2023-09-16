using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class ZoneDataDTO
    {
        public Guid GUID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string GeoJSON { get; set; } = string.Empty;
        public List<ParkingAreaDTO> ParkingAreas { get; set; } = new List<ParkingAreaDTO>();
    }
}
