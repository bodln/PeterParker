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

        public async void Add(TicketDTO request)
        {
            ParkingSpace parkingSpace = await context.ParkingSpaces
                .Where(ps => ps.GUID == request.GUID)
                .Include(ps => ps.Vehicle)
                    .ThenInclude(v => v.User)
                        .ThenInclude(u => u.Tickets)
                .FirstOrDefaultAsync();

            if (parkingSpace == null)
            {
                throw new NotFoundException("Parking space not found.");
            }

            Ticket alreadyExisting = await context.Tickets
                .Where(t => t.Registration == parkingSpace.Vehicle.Registration)
                .Where(t => t.ZoneGuid == request.ZoneGuid)
                .Where(t => t.Issued.AddHours(24) > DateTime.Now)
                .FirstOrDefaultAsync();

            Vehicle vehicle = parkingSpace.Vehicle;
            Zone zone = await context.Zones.Where(z => z.GUID == request.GUID).FirstOrDefaultAsync();

            if (zone == null)
            {
                throw new NotFoundException("Zone not found.");
            }

            if (alreadyExisting != null)
            {
                throw new DuplicateObjectException($"There is already a ticket for the vehicle: {vehicle.Registration}, within {zone.Name}, issued less than 24 hours ago.");
            }

            User user = vehicle.User;

            if (request.ParkingSpaceGuid == null || request.ZoneGuid == null)
            {
                throw new MissingParametersException("Missing parameters for ticket creation.");
            }
            Ticket ticket = mapper.Map<Ticket>(request);

            ticket.Issued = DateTime.Now;
            ticket.Registration = parkingSpace.Vehicle.Registration;

            user.Tickets.Add(ticket);

            context.Tickets.Add(ticket);
            context.SaveChanges();
        }

        public List<TicketDTO> GetAll()
        {
            return mapper.Map<List<TicketDTO>>(context.Tickets.ToList()); 
        }

        public async Task Settle(TicketDTO ticketDTO)
        {
            //Ticket ticket = await context.Tickets
            //    .Where(t => t.GUID = ticketDTO.GUID);
        }   
    }
}
