using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Exceptions
{
    internal class VehicleAlreadyParkedException : ApplicationException
    {
        public VehicleAlreadyParkedException(string vehicleRegistration) : base($"Vehicle with the registration {vehicleRegistration}, is already parked.")
        {

        }
    }
}
