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

        public async Task<List<ParkingSpaceDTO>> GetAllByGeoJSON(string request)
        {
            Zone zone = await context.Zones
            .Where(z => z.GeoJSON == request)
            .Include(z => z.ParkingSpaces)
                .ThenInclude(ps => ps.Vehicle)
            .FirstOrDefaultAsync();

            if (zone == null)
                throw new NotFoundException($"Zone not found.");

            List<ParkingSpaceDTO> parkingSpaceDTOs = new List<ParkingSpaceDTO>();

            foreach (ParkingSpace parkingSpace in zone.ParkingSpaces)
            {
                parkingSpaceDTOs.Add(mapper.Map<ParkingSpaceDTO>(parkingSpace));
            }

            return parkingSpaceDTOs;
        }
    }
}
