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

        public async Task AddParkingArea(ParkingAreaDTO request)
        {
            try
            {
                ParkingArea parkingArea = mapper.Map<ParkingArea>(request);
                parkingArea.GUID = Guid.NewGuid();

                context.ParkingAreas.Add(parkingArea);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
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
                logger.LogError(e.Message);
                throw;
            }
        }
    }
}
