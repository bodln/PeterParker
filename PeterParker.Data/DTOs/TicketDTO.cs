using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class TicketDTO
    {
        public string Name { get; set; } = string.Empty; // Combo of zoneid and parkingspaceid or somesuch
        public Guid GUID { get; set; }
        public bool Paid { get; set; }
        public int ZoneId { get; set; }
        public int ParkingSpaceId { get; set; }
        public int Fine { get; set; }
    }
}
