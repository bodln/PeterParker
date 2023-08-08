using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Exceptions
{
    internal class TicketMissingParametersException : ApplicationException
    {
        public TicketMissingParametersException() : base("Missing parameters for ticket creation.")
        {

        }
    }
}
