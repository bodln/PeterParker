using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Exceptions
{
    public class InvalidRefreshToken : ApplicationException
    {
        public InvalidRefreshToken() : base("Refresh token is invalid")
        {

        }
    }
}
