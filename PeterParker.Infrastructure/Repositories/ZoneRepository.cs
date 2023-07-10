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

        public bool Add(ZoneDTO request)
        {
            try
            {
                Zone zone = mapper.Map<Zone>(request);
                context.Zones.Add(zone);
                context.SaveChanges();

                List<ParkingSpace> parkingSpaces = new List<ParkingSpace>();

                for (int i = 0; i < zone.TotalSpaces; i++)
                {
                    // a method for different parkingspaces to have
                    // different streets in the same zone must be implemented lazy rn
                    parkingSpaces.Add(new ParkingSpace
                    {
                        Street = "Implement this",
                        Vehicle = null,
                        Garage = null,
                        Number = i + 1
                    });
                }

                context.ParkingSpaces.AddRange(parkingSpaces);
                context.SaveChanges();

                zone.ParkingSpaces = parkingSpaces;
                context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);

                return false;
            }
        }

        public List<Zone> GetAll()
        {
            try
            {
                return context.Zones.Include(z => z.ParkingSpaces).ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);

                return null;
            }
        }
    }
}
