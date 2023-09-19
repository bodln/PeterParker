using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeterParker.Data;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.Data.Paging;
using PeterParker.Infrastructure.Exceptions;
using PeterParker.Infrastructure.Interfaces;

namespace PeterParker.Infrastructure.Repositories
{
    public class ParkingAreaRepository : IParkingAreaRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<ParkingAreaRepository> logger;
        private readonly IParkingSpaceRepository parkingSpaceRepository;

        public ParkingAreaRepository(DataContext context,
            IMapper mapper,
            ILogger<ParkingAreaRepository> logger,
            IParkingSpaceRepository parkingSpaceRepository)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
            this.parkingSpaceRepository = parkingSpaceRepository;
        }

        public async Task<ParkingArea> CreateParkingArea(Guid zoneGuid, ParkingAreaDTO request)
        {
            string[] parkingAreaTypes = { "garage", "underground", "lot" };

            if (!parkingAreaTypes.Contains(request.Type))
            {
                logger.LogWarning("The requested area type is not valid.");
                throw new MissingParametersException("Missing parameters for parking area creation.");
            }
            try
            {
                ParkingArea parkingArea = mapper.Map<ParkingArea>(request);
                parkingArea.GUID = Guid.NewGuid();

                logger.LogInformation("Adding new area.");

                context.ParkingAreas.Add(parkingArea);
                context.SaveChanges();

                logger.LogInformation("Success.");
                logger.LogInformation("Adding parking spaces to area.");

                request.GUID = parkingArea.GUID;

                List<ParkingSpace> parkingSpaces = await parkingSpaceRepository
                    .AddParkingSpacesToArea(zoneGuid, request);

                parkingArea.ParkingSpaces = parkingSpaces;

                context.SaveChanges();

                logger.LogInformation("Success.");

                return parkingArea;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<ParkingAreaDTO>> GetAllParkingAreas()
        {
            try
            {
                logger.LogInformation("Getting all areas.");

                List<ParkingArea> parkingAreas = await context.ParkingAreas
                        .Include(pa => pa.ParkingSpaces)
                            .ThenInclude(ps => ps.Vehicle)
                                .ThenInclude(v => v.User)
                        .OrderBy(pa => pa.Address)
                        .ToListAsync();

                return mapper.Map<List<ParkingAreaDTO>>(parkingAreas);
            }
            catch (Exception e)
            {
                logger.LogInformation("Failed.");
                throw new Exception(e.Message);
            }
        }

        public async Task<List<ParkingAreaDTO>> SearchAreas(string request)
        {
            List<ParkingArea> parkingAreas = await context.ParkingAreas
                .Include(pa => pa.ParkingSpaces)
                    .ThenInclude(ps => ps.Vehicle)
                .Where(pa => pa.Address.Contains(request, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(pa => pa.Address)
                .ToListAsync();

            //int pageSize = 5;
            //parkingAreas = PaginatedList<ParkingArea>
            //    .Create(parkingAreas.AsQueryable(),
            //    pageNumber ?? 1,
            //    pageSize);

            return mapper.Map<List<ParkingAreaDTO>>(parkingAreas);
        }

        public async Task DeleteArea(ParkingAreaDTO parkingAreaDTO)
        {
            logger.LogInformation("Deleting area.");

            ParkingArea parkingArea = await context.ParkingAreas
                .Where(pa => pa.GUID == parkingAreaDTO.GUID)
                .Include(pa => pa.ParkingSpaces)
                .FirstOrDefaultAsync();

            context.ParkingSpaces.RemoveRange(parkingArea.ParkingSpaces);
            context.ParkingAreas.Remove(parkingArea);
            context.SaveChanges();

            logger.LogInformation("Success.");
        }
    }
}
