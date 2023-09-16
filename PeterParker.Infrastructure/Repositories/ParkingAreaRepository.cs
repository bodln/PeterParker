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
    public class ParkingAreaRepository : IParkingAreaRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<ParkingAreaRepository> logger;

        public ParkingAreaRepository(DataContext context,
            IMapper mapper,
            ILogger<ParkingAreaRepository> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ParkingArea> CreateParkingArea(ParkingAreaDTO request)
        {
            string[] parkingAreaTypes = { "garage", "underground", "lot" };

            if (!parkingAreaTypes.Contains(request.Type))
            {
                throw new MissingParametersException("Missing parameters for parking area creation.");
            }
            try
            {
                ParkingArea parkingArea = mapper.Map<ParkingArea>(request);
                parkingArea.GUID = Guid.NewGuid();

                context.ParkingAreas.Add(parkingArea);
                context.SaveChanges();

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
                List<ParkingArea> parkingAreas = await context.ParkingAreas
                        .Include(pa => pa.ParkingSpaces)
                            .ThenInclude(ps => ps.Vehicle)
                                .ThenInclude(v => v.User)
                        .ToListAsync();

                return mapper.Map<List<ParkingAreaDTO>>(parkingAreas);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task DeleteArea(ParkingAreaDTO parkingAreaDTO)
        {
            ParkingArea parkingArea = await context.ParkingAreas
                .Where(pa => pa.GUID == parkingAreaDTO.GUID)
                .Include(pa => pa.ParkingSpaces)
                .FirstOrDefaultAsync();

            context.ParkingSpaces.RemoveRange(parkingArea.ParkingSpaces);
            context.ParkingAreas.Remove(parkingArea);
            context.SaveChanges();
        }
    }
}
