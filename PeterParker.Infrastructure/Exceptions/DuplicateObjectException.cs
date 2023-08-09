using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Exceptions
{
    public class DuplicateObjectException : ApplicationException
    {
        public DuplicateObjectException(string message) : base(message)
        {

        }
    }
}
