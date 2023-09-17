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
        private readonly IParkingAreaRepository parkingAreaRepository;

        public ZoneRepository(DataContext context,
            IMapper mapper,
            ILogger<ZoneRepository> logger,
            IParkingAreaRepository parkingAreaRepository) //: base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
            this.parkingAreaRepository = parkingAreaRepository;
        }

        public async Task<ZoneDataDTO> Add(ZoneDTO request)
        {
            if (request.GeoJSON == "")
                throw new MissingParametersException("Missing parameters for zone creation.");

            int zonesCount = await context.Zones.CountAsync();

            Zone zone = mapper.Map<Zone>(request);
            zone.GUID = Guid.NewGuid();
            zone.Name = $"Zone {zonesCount}";
            context.Zones.Add(zone);
            context.SaveChanges();

            logger.LogInformation("Zone created.");

            ZoneDataDTO response = mapper.Map<ZoneDataDTO>(zone);

            return response;
        }

        public async Task<ZoneDataDTO> Update(ZoneDTO zoneDTO)
        {
            logger.LogInformation("Getting zone.");

            Zone zone = await GetZoneByGuid(zoneDTO.GUID);

            logger.LogInformation("Zone found.");

            zone.Name = zoneDTO.Name;
            zone.GeoJSON = zoneDTO.GeoJSON;

            context.SaveChanges();

            logger.LogInformation("Zone updated.");

            return mapper.Map<ZoneDataDTO>(zone);
        }

        public async Task Delete(ZoneDTO zoneDTO)
        {
            logger.LogInformation("Getting zone.");

            Zone zone = await GetZoneByGuid(zoneDTO.GUID);

            logger.LogInformation("Zone found.");

            foreach (ParkingArea parkingArea in zone.ParkingAreas)  
            {
                context.ParkingSpaces.RemoveRange(parkingArea.ParkingSpaces);
            }

            logger.LogInformation("Deleting zone.");

            context.ParkingAreas.RemoveRange(zone.ParkingAreas);
            context.Zones.Remove(zone);
            context.SaveChanges();

            logger.LogInformation("Deletion successful");
        }

        public async Task<List<ZoneDataDTO>> GetAll()
        {
            logger.LogInformation("Getting all zones.");

            List<Zone> zones = await GetAllZones();

            logger.LogInformation("Success.");

            List<ZoneDataDTO> zoneDataDTOs = mapper.Map<List<ZoneDataDTO>>(zones);

            return zoneDataDTOs;
        }

        public async Task AddArea(Guid zoneGuid, ParkingAreaDTO parkingAreaDTO)
        {
            Zone zone = await GetZoneByGuid(zoneGuid);

            ParkingArea parkingArea = await parkingAreaRepository.CreateParkingArea(zoneGuid,parkingAreaDTO);

            zone.ParkingAreas.Add(parkingArea);
            context.SaveChanges();
        }

        public async Task<List<Zone>> GetAllZones()
        {
              List<Zone> zones = await context.Zones
                .Include(z => z.ParkingAreas)
                    .ThenInclude(pa => pa.ParkingSpaces)
                        .ThenInclude(ps => ps.Vehicle)
                            .ThenInclude(v => v.User)
                .OrderBy(z => z.Name)
                .ToListAsync();

            foreach (Zone zone in zones)
            {
                zone.ParkingAreas = zone.ParkingAreas.OrderBy(sa => sa.Address).ToList();
            }

            return zones;
        }

        public async Task<Zone> GetZoneByGuid(Guid zoneGuid)
        {
            logger.LogInformation("Searching for zone.");

            Zone zone = await context.Zones
                .Include(z => z.ParkingAreas)
                    .ThenInclude(pa => pa.ParkingSpaces)
                        .ThenInclude(ps => ps.Vehicle)
                            .ThenInclude(v => v.User)
                .FirstOrDefaultAsync(z => z.GUID == zoneGuid);

            if (zone == null)
            {
                throw new NotFoundException($"Zone with GUID: {zoneGuid}, is not found.");
            }

            logger.LogInformation("Zone found.");

            return zone;
        }
    }
}
