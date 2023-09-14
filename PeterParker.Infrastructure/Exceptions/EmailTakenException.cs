using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Exceptions
{
    public class EmailTakenException : ApplicationException
    {
        public EmailTakenException(string message) : base(message)
        {

        }
    }
}
