using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeterParker.Data;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository//, Repository<Ticket>
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<TicketRepository> logger;

        public TicketRepository(DataContext context,
            IMapper mapper,
            ILogger<TicketRepository> logger) //: base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public bool Add(TicketDTO request)
        {
            try
            {
                Ticket ticket = mapper.Map<Ticket>(request);
                context.Tickets.Add(ticket);

                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return false;
            }

        }

        public async Task<List<Ticket>> GetAll()
        {
            try
            {
                return await context.Tickets.ToListAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return null;

                throw;
            }
        }
    }
}
