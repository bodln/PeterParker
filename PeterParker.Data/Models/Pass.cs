﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.Models;

public class Pass
{
    public int Id { get; set; }
    public DateTime TimeOfSale { get; set; }
    public List<Zone> ZoneIds { get; set; } = new List<Zone>(); 
    public DateTime Expiration { get; set; }
}
