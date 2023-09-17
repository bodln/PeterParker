using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class Subscription
{
    public int Id { get; set; }
    public Guid GUID { get; set; }
    public DateTime Expiration { get; set; }
    public float Price { get; set; }
}
