using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Exceptions
{
    internal class ParkingSpaceTakenException : ApplicationException
    {
        public ParkingSpaceTakenException() : base("Parking space is already taken.")
        {

        }
    }
}
