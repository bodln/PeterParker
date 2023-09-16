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

            if(context.Zones.Where(z => z.Name == request.Name).FirstOrDefault() != null)
            {
                throw new DuplicateObjectException("A zone with this name already exists.");
            }

            Zone zone = mapper.Map<Zone>(request);
            zone.GUID = Guid.NewGuid();
            context.Zones.Add(zone);
            context.SaveChanges();
            ZoneDataDTO response = mapper.Map<ZoneDataDTO>(zone);

            return response;
        }

        public async Task<List<ZoneDataDTO>> GetAll()
        {
            List<Zone> zones = await context.Zones
                .Include(z => z.ParkingAreas)
                        .ThenInclude(pa => pa.ParkingSpaces)
                            .ThenInclude(ps => ps.Vehicle)
                                .ThenInclude(v => v.User)
                .ToListAsync();

            List<ZoneDataDTO> zoneDataDTOs = mapper.Map<List<ZoneDataDTO>>(zones);

            return zoneDataDTOs;
        }

        public async Task AddAreaByGuid(Guid zoneGuid, Guid areaGuid)
        {
            Zone zone = context.Zones.Where(z => z.GUID == zoneGuid)
                .Include(z => z.ParkingAreas)
                .FirstOrDefault();

            if (zone == null)
            {
                throw new NotFoundException($"Zone with GUID: {zoneGuid}, is not found.");
            }

            ParkingArea parkingArea = context.ParkingAreas
                .Where(pa => pa.GUID == areaGuid)
                .FirstOrDefault();

            if (parkingArea == null)
            {
                throw new NotFoundException($"Parking area with GUID: {areaGuid}, is not found.");
            }

            zone.ParkingAreas.Add(parkingArea);
            context.SaveChanges();
        }
    }
}
