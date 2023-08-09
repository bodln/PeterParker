using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeterParker.Data;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.Infrastructure.Exceptions;
using PeterParker.Infrastructure.Interfaces;

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

        public void Add(TicketDTO request)
        {
            if (request.ParkingSpaceId == null || request.ZoneId == null)
            {
                throw new MissingParametersException("Missing parameters for ticket creation.");
            }
            Ticket ticket = mapper.Map<Ticket>(request);
            ticket.Zone = context.Zones.Where(z => z.Id == request.ZoneId).FirstOrDefault();
            context.Tickets.Add(ticket);
            context.SaveChanges();
        }

        public List<Ticket> GetAll()
        {
            return context.Tickets
                .Include(t => t.Zone)
                .ToList();
        }
    }
}
