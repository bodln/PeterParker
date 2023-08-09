using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Exceptions
{
    public class MissingParametersException : ApplicationException
    {
        public MissingParametersException(string message) : base (message)
        {

        }
    }
}
