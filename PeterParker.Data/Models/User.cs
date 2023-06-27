using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<string> Vehicles { get; set; } = new List<string>();
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    public Pass? Pass { get; set; }
    public Subscription? Subscription { get; set; }
}
