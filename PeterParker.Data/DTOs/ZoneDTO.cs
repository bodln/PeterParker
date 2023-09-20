using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class ZoneDTO
    {
        public Guid GUID { get; set; }
        public string Name { get; set; } = string.Empty;
        public float HourlyRate { get; set; }
        public string GeoJSON { get; set; } = string.Empty;
    }
}
