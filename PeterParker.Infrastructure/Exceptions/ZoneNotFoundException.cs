using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Exceptions
{
    public class ZoneNotFoundException : ApplicationException
    {

        public ZoneNotFoundException(string GeoJSON) : base($"Zone {GeoJSON} has not been found.")
        {
        }
    }
}
