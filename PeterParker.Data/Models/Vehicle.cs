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
        [Required]
        [Unique]
        public string Registration { get; set; } = string.Empty;
        // My thoughts are the following:
        // The owner(User) of the Vehicle should be kept here opposing the previous arrangement of the Vehicle list
        // being kept by the User. Purpose of this change is that when a User or an Inspector goes through the list of 
        // ParkingSpaces in a Zone they can take the following path and collect data along the way:
        // Zone -> ParkingSpaces -> Vehicle -> User
        // In case of the request for all the Vehicles belonging to a User a detour must be taken, but
        // as of the time of my writing this it is the only reason for taking that detour and so
        // the aforementioned path is much more commonly taken. (TableOptimisationV1)
        public User User { get; set; }
    }
}
