using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class PassDTO
    {
        public Guid GUID { get; set; }
        public DateTime TimeOfSale { get; set; }
        public List<ZoneDTO> Zones { get; set; }
        public DateTime Expiration { get; set; }
        public float Price { get; set; }
    }
}
