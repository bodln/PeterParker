using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Exceptions
{
    public class NoParkingPermitException : ApplicationException
    {
        public NoParkingPermitException(string message) : base(message)
        {

        }
    }
}
