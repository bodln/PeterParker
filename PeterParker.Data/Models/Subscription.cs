using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class Subscription
{
    public int Id { get; set; }
    public DateTime Expiration { get; set; }
    public List<Zone> Zones { get; set; } = new List<Zone>();
    public Vehicle Vehicle { get; set; }
}
