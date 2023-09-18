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
            if (request.ParkingSpaceGuid == null || request.ZoneGuid == null)
            {
                throw new MissingParametersException("Missing parameters for ticket creation.");
            }
            Ticket ticket = mapper.Map<Ticket>(request);
            context.Tickets.Add(ticket);
            context.SaveChanges();
        }

        public List<TicketDTO> GetAll()
        {
            return mapper.Map<List<TicketDTO>>(context.Tickets.ToList()); // map...
        }
    }
}
