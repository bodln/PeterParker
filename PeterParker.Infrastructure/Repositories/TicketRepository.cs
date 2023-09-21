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

        public async Task Add(TicketDTO request)
        {
            if (request.ParkingSpaceGuid == null || request.ZoneGuid == null)
            {
                throw new MissingParametersException("Missing parameters for ticket creation.");
            }
            ParkingSpace parkingSpace = await context.ParkingSpaces
                .Where(ps => ps.GUID == request.ParkingSpaceGuid)
                .Include(ps => ps.Vehicle)
                    .ThenInclude(v => v.User)
                        .ThenInclude(u => u.Tickets)
                .FirstOrDefaultAsync();

            if (parkingSpace == null)
            {
                throw new NotFoundException("Parking space not found.");
            }

            if (parkingSpace.Vehicle == null)
            {
                throw new NotFoundException("There is no car parked here.");
            }

            Ticket alreadyExisting = await context.Tickets
                .Where(t => t.Registration == parkingSpace.Vehicle.Registration &&
                t.ZoneGuid == request.ZoneGuid &&
                t.Issued.AddHours(24) > DateTime.Now)
                .FirstOrDefaultAsync();

            Vehicle vehicle = parkingSpace.Vehicle;
            Zone zone = await context.Zones.Where(z => z.GUID == request.ZoneGuid).FirstOrDefaultAsync();

            if (zone == null)
            {
                throw new NotFoundException("Zone not found.");
            }

            if (alreadyExisting != null)
            {
                throw new DuplicateObjectException($"There is already a ticket for the vehicle: {vehicle.Registration}, within {zone.Name}, issued less than 24 hours ago.");
            }

            User user = vehicle.User;

            Ticket ticket = new Ticket(); 
            ticket = mapper.Map<Ticket>(request);

            ticket.GUID = Guid.NewGuid();
            ticket.Issued = DateTime.Now;
            ticket.Registration = vehicle.Registration;

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
            Ticket ticket = await context.Tickets
                .Where(t => t.GUID == ticketDTO.GUID)
                .FirstOrDefaultAsync();

            if (ticket == null)
            {
                throw new NotFoundException("Ticket not found.");
            }

            User user = (await context.Vehicles
                .Where(v => v.Registration == ticketDTO.Registration)
                .Include(v => v.User)
                    .ThenInclude(u => u.Tickets)
                .FirstOrDefaultAsync()).User;

            if (user == null)
            {
                throw new NotFoundException($"No vehicle with registration: {ticketDTO.Registration} found.");
            }

            ticket.Paid = true;
            ticket.Settled = DateTime.Now;
            ticket.SettleReason = ticketDTO.SettleReason;
            user.Tickets.Remove(ticket);

            context.SaveChanges();
        }   
    }
}
