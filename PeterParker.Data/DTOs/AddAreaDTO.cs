using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class AddAreaDTO
    {
        public Guid ZoneGUID { get; set; }
        public ParkingAreaDTO ParkingArea { get; set; }
    }
}
