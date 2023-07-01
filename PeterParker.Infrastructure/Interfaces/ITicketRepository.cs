using PeterParker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Interfaces
{
    internal interface ITicketRepository : IRepository<Ticket>
    {
    }
}
