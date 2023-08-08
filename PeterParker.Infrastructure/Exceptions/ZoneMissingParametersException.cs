using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Exceptions
{
    public class ZoneMissingParametersException : ApplicationException
    {
        public ZoneMissingParametersException() : base("There are missing parameters for zone creation.")
        {
                
        }
    }
}
