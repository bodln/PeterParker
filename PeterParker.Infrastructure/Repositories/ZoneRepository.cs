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
    public class ZoneRepository : IZoneRepository //, Repository<Zone>
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<ZoneRepository> logger;

        public ZoneRepository(DataContext context,
            IMapper mapper,
            ILogger<ZoneRepository> logger) //: base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ZoneDataDTO> Add(ZoneDTO request)
        {
            if (request.GeoJSON == "")
                throw new MissingParametersException("Missing parameters for zone creation.");
            Zone zone = mapper.Map<Zone>(request);
            zone.GUID = new Guid();
            context.Zones.Add(zone);
            await context.SaveChangesAsync();
            ZoneDataDTO response = mapper.Map<ZoneDataDTO>(zone);

            return response;
        }

        public async Task<List<Zone>> GetAll()
        {
            // make this return ZoneDataDTO
            return await context.Zones.Include(z => z.ParkingAreas)
                        .ThenInclude(pa => pa.ParkingSpaces)
                            .ThenInclude(ps => ps.Vehicle)
                        .ToListAsync(); 
        }
    }
}
