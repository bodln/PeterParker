using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class ZoneDTO
    {
        public string GeoJSON { get; set; }
        public int TotalSpaces { get; set; }
        public int FreeSpaces { get; set; }
        public List<ParkingSpaceDTO> ParkingSpaces { get; set; } = new List<ParkingSpaceDTO>();
    }
}
