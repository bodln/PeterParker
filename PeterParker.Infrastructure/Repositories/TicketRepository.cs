using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeterParker.Data;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.Infrastructure.Exceptions;
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

        public void Add(TicketDTO request)
        {
            if (request.ParkingSpaceId == null || request.ZoneId == null)
            {
                throw new TicketMissingParametersException();
            }

            try
            {
                Ticket ticket = mapper.Map<Ticket>(request);
                ticket.Zone = context.Zones.Where(z => z.Id == request.ZoneId).FirstOrDefault();
                context.Tickets.Add(ticket);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ex.Message);
            }

        }

        public List<Ticket> GetAll()
        {
            try
            {
                return context.Tickets
                    .Include(t => t.Zone)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ex.Message);
            }
        }
    }
}
