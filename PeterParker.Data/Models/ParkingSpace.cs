using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class ParkingSpace
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public Vehicle? Vehicle { get; set; }
    // Here i decided to have a list of ParkingSpaces in zone rather than each ParkingSpace
    // keep track of its zone. What i have in mind with this is that when an Inspector
    // checks all of the parking spaces in a zone and wants to issue a ticket the Database
    // will already know which zone the ParkingSpace belongs to and can access the Vehicle
    // and by extension its owner (User) (TableOptimisationV1)
    //public Zone Zone { get; set; } 
    public Garage? Garage { get; set; } // Reference the Garage class
    public int Number { get; set; } // in zone
}
