using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Exceptions
{
    public class IncorrectLoginInfoException : ApplicationException
    {
        public IncorrectLoginInfoException() : base("Incorrect Login info sent.")
        {

        }
    }
}
