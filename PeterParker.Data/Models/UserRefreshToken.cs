using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models
{
    public class UserRefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
        public bool IsActive { 
            get
            {
                return Expires < DateTime.UtcNow;
            }
        }
        public string IpAddress { get; set; }
        public bool IsInvalidated { get; set; }
        public User User { get; set; }
    }
}
