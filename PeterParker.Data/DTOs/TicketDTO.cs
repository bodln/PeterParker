using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class TicketDTO
    {
        public Guid GUID { get; set; }
        public bool Paid { get; set; }
        public Guid ZoneGuid { get; set; }
        public Guid ParkingSpaceGuid{ get; set; }
        public string Reason { get; set; }
        public int Fine { get; set; }
    }
}
