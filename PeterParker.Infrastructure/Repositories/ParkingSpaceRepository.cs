using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeterParker.Data;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<ParkingSpaceDTO> GetAllByGeoJSON(string request)
        {
            try
            {
                Zone zone = context.Zones
                .Where(z => z.GeoJSON == request)
                .Include(z => z.ParkingSpaces)
                    .ThenInclude(ps => ps.Vehicle)
                .FirstOrDefault();

                List<ParkingSpaceDTO> parkingSpaceDTOs = new List<ParkingSpaceDTO>();

                foreach (ParkingSpace parkingSpace in zone.ParkingSpaces)
                {
                    parkingSpaceDTOs.Add(mapper.Map<ParkingSpaceDTO>(parkingSpace));
                }

                return parkingSpaceDTOs;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);

                return null;
            }
        } 
    }
}
