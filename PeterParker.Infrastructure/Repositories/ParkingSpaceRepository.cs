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
    public class ParkingSpaceRepository : IParkingSpaceRepository//, Repository<ParkingSpace>
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<ParkingSpaceRepository> logger;

        public ParkingSpaceRepository(DataContext context,
            IMapper mapper,
            ILogger<ParkingSpaceRepository> logger) //: base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }
        //Useless
        public async Task<List<ParkingSpaceDTO>> GetAllByGeoJSON(string request)
        {
            Zone zone = await context.Zones
            .Where(z => z.GeoJSON == request)
            .Include(z => z.ParkingAreas)
                .ThenInclude(pa => pa.ParkingSpaces)
                    .ThenInclude(ps => ps.Vehicle)
            .FirstOrDefaultAsync();

            if (zone == null)
                throw new NotFoundException($"Zone not found.");

            List<ParkingSpaceDTO> parkingSpaceDTOs = new List<ParkingSpaceDTO>();

            return parkingSpaceDTOs;
        }

        public async Task<List<ParkingSpace>> AddParkingSpacesToArea(Guid zoneGuid, ParkingAreaDTO parkingAreaDTO)
        {
            Zone zone = await GetZoneByGuid(zoneGuid);

            //ParkingArea parkingArea = await GetParkingAreaByGuid(parkingAreaDTO.GUID);

            int numberOfSpacesInZone = 0;

            foreach (ParkingArea pa in zone.ParkingAreas)
            {
                numberOfSpacesInZone += pa.ParkingSpaces.Count();
            }

            List<ParkingSpace> parkingSpaces = new List<ParkingSpace>();

            for (int i = 0; i < parkingAreaDTO.NumberOfSpaces; i++)
            {
                ParkingSpace parkingSpace = new ParkingSpace
                {
                    GUID = Guid.NewGuid(),
                    Number = ++numberOfSpacesInZone
                };

                parkingSpaces.Add(parkingSpace);
            }

            context.ParkingSpaces.AddRange(parkingSpaces);
            context.SaveChanges();

            return parkingSpaces;
        }

        public async Task AddParkingSpaceToAreaByGuid(Guid request)
        {
            ParkingArea parkingArea = await GetParkingAreaByGuid(request);

            if (parkingArea == null)
            {
                throw new NotFoundException("Parking area not found.");
            }

            ParkingSpace parkingSpace = new ParkingSpace();
            parkingSpace.GUID = Guid.NewGuid();
            parkingSpace.Number = parkingArea.ParkingSpaces.Count() + 1;
            parkingArea.ParkingSpaces.Add(parkingSpace);

            context.ParkingSpaces.Add(parkingSpace);
            context.SaveChanges();
        }

        public async Task<List<ParkingSpaceDTO>> GetAllByGuid(Guid request)
        {
            try
            {
                ParkingArea parkingArea = await GetParkingAreaByGuid(request);
                return mapper.Map<List<ParkingSpaceDTO>>(parkingArea.ParkingSpaces);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new Exception(e.Message);
            }
        }

        public async Task<ParkingArea> GetParkingAreaByGuid(Guid request)
        {
            ParkingArea parkingArea = await context.ParkingAreas
                    .Where(pa => pa.GUID == request)
                    .Include(pa => pa.ParkingSpaces)
                        .ThenInclude(ps => ps.Vehicle)
                    .FirstOrDefaultAsync();

            return parkingArea;
        }

        public async Task<Zone> GetZoneByGuid(Guid request)
        {
            Zone zone = await context.Zones
            .Where(z => z.GUID == request)
            .Include(z => z.ParkingAreas)
                .ThenInclude(pa => pa.ParkingSpaces)
                    .ThenInclude(ps => ps.Vehicle)
            .FirstOrDefaultAsync();

            return zone;
        }
    }
}
