using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public Guid GUID { get; set; }
        public string Registration { get; set; } = string.Empty;
        public Guid ParkingSpaceGuid { get; set; } = Guid.Empty;
        public User User { get; set; }
    }
}
