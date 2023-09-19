using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure
{
    public class ErrorResponse : Exception
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
};
