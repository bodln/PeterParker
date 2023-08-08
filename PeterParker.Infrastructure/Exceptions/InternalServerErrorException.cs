using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Exceptions
{
    public class InternalServerErrorException : ApplicationException
    {
        public InternalServerErrorException() : base("Something went wrong.")
        {

        }

        public InternalServerErrorException(string message) : base(message)
        {

        }
    }
}
