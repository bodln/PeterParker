using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeterParker.Data;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.Infrastructure.Exceptions;
using PeterParker.Infrastructure.Interfaces;

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
        public async Task AddVehicle(VehicleDTO request)
        {
            if (await context.Vehicles.AnyAsync(v => v.Registration == request.Registration))
            {
                throw new DuplicateObjectException($"A vehicle with the registration: {request.Registration}, already exists.");
            }

            Vehicle vehicle = mapper.Map<Vehicle>(request);
            vehicle.User = await userManager.FindByEmailAsync(request.UserEmail);
            vehicle.GUID = Guid.NewGuid();

            if (vehicle.User == null)
            {
                throw new NotFoundException("The owner for this vehicle is not found.");
            }

            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        public async Task<List<VehicleDTO>> GetAllVehiclesForUserByEmail(string request)
        {
            var user = await userManager.FindByEmailAsync(request);

            if (user == null)
            {
                throw new NotFoundException("The owner of the vehicles could not be found.");
            }

            List<Vehicle> vehicles = await context.Vehicles.Where(v => v.User == user).ToListAsync();

            List<VehicleDTO> vehiclesDTO = mapper.Map<List<VehicleDTO>>(vehicles);

            //foreach (var vehicle in vehicles)
            //{
            //    vehiclesDTO.Add(mapper.Map<VehicleDTO>(vehicle));
            //}

            return vehiclesDTO;
        }

        public async Task DeleteVehicle(string request)
        {
            Vehicle vehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.Registration == request);

            if (vehicle == null)
            {
                throw new NotFoundException($"The vehicles with the registration: {request}, could not be found.");
            }

            ParkingSpace parkingSpace = await context.ParkingSpaces
                .FirstOrDefaultAsync(ps => ps.Vehicle == vehicle);

            if (parkingSpace != null)
            {
                parkingSpace.Vehicle = null;
                await context.SaveChangesAsync();
            }

            context.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();
        }

        public async Task ParkVehicle(Guid parkingSpaceGuid, string registration)
        {
            Vehicle vehicle = await context.Vehicles.Where(v => v.Registration == registration).FirstOrDefaultAsync();

            if (vehicle == null)
            {
                throw new NotFoundException($"The vehicles with the registration: {registration}, could not be found.");
            }

            ParkingSpace parkingSpace = await context.ParkingSpaces
                .Where(ps => ps.GUID == parkingSpaceGuid)
                .FirstOrDefaultAsync();

            if (parkingSpace.Vehicle == null)
            {
                parkingSpace.Vehicle = vehicle;
            }
            else
            {
                throw new ParkingSpaceTakenException();
            }

            await context.SaveChangesAsync();
        }
        // I'm thinking that on the frontend the user knows where his vehicle is parked
        // so when unparking just send that info with no need of sending the registration
        public async Task UnparkVehicle(Guid parkingSpaceGuid)
        {
            ParkingSpace parkingSpace = await context.ParkingSpaces
                .Where(ps => ps.GUID == parkingSpaceGuid)
                .Include(ps => ps.Vehicle)
                .FirstOrDefaultAsync();

            if (parkingSpace == null)
            {
                throw new NotFoundException($"The parking space with number: {parkingSpace.Number}, could not be found.");
            }

            parkingSpace.Vehicle = null;
            await context.SaveChangesAsync();
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
