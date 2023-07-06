using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly DataContext context;
        private readonly ILogger<VehicleRepository> logger;

        public VehicleRepository(IMapper mapper,
            UserManager<User> userManager,
            DataContext context,
            ILogger<VehicleRepository> logger)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.context = context;
            this.logger = logger;
        }
        public async Task<bool> AddVehicle(VehicleDTO request)
        {
            try
            {
                if (await context.Vehicles.AnyAsync(v => v.Registration == request.Registration))
                {
                    logger.LogInformation("There already exists a Vehicle by that registration.");
                    return false;
                }

                Vehicle vehicle = mapper.Map<Vehicle>(request);
                vehicle.User = await userManager.FindByEmailAsync(request.UserEmail);
                context.Vehicles.Add(vehicle);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return false;
            }
        }

        public async Task<List<VehicleDTO>> GetAllVehiclesForUserByEmail(string request)
        {
            var user = await userManager.FindByEmailAsync(request);
            List<Vehicle> vehicles = await context.Vehicles.Where(v => v.User == user).ToListAsync();

            List<VehicleDTO> vehiclesDTO = new List<VehicleDTO>();

            foreach (var vehicle in vehicles)
            {
                vehiclesDTO.Add(mapper.Map<VehicleDTO>(vehicle));
            }

            return vehiclesDTO;
        }

        public async Task<bool> DeleteVehicle(string request)
        {
            try
            {
                Vehicle vehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.Registration == request);
                context.Vehicles.Remove(vehicle);
                return true;
            }
            catch (Exception e)
            {
                logger.LogInformation(e.Message);
                return false;
            }
        }

        public List<VehicleDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public VehicleDTO GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(VehicleDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
