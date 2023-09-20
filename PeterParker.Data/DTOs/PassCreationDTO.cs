using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Data.DTOs
{
    public class PassCreationDTO
    {
        public List<Guid> ZoneGUIDs { get; set; }
        public int Hours { get; set; }
    }
}
