using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        //public User Owner { get; set; } = null;
        //[Unique]
        public string Registration { get; set; } = string.Empty;
    }
}
