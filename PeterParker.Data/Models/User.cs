using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class User : IdentityUser
{
    //public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    //public string Email { get; set; } = string.Empty;
    //public byte[] PasswordHash { get; set; }
    //public byte[] PasswordSalt { get; set; }
    public string HomeAddress { get; set; } = string.Empty;
    //public string PhoneNumber { get; set; } = string.Empty;
    public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    public Pass? Pass { get; set; }
    public Subscription? Subscription { get; set; }
}
