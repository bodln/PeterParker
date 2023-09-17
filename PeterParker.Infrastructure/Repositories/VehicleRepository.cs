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

            logger.LogInformation("Adding vehicle.");

            context.Vehicles.Add(vehicle);
            context.SaveChanges();

            logger.LogInformation("Vehicle added.");
        }

        public async Task<List<VehicleDTO>> GetAllVehiclesForUserByEmail(string request)
        {
            logger.LogInformation("Getting vehicle owner.");

            var user = await userManager.FindByEmailAsync(request);

            if (user == null)
            {
                throw new NotFoundException("The owner of the vehicles could not be found.");
            }

            logger.LogInformation("Owner found.");
            logger.LogInformation("Getting vehicles.");

            List<Vehicle> vehicles = await context.Vehicles.Where(v => v.User == user).ToListAsync();

            logger.LogInformation("Vehicles found.");

            List<VehicleDTO> vehiclesDTO = mapper.Map<List<VehicleDTO>>(vehicles);

            return vehiclesDTO;
        }

        public async Task DeleteVehicle(string request)
        {
            logger.LogInformation("Getting vehicles.");

            Vehicle vehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.Registration == request);

            if (vehicle == null)
            {
                throw new NotFoundException($"The vehicles with the registration: {request}, could not be found.");
            }

            logger.LogInformation("Vehicle found.");

            ParkingSpace parkingSpace = await context.ParkingSpaces
                .FirstOrDefaultAsync(ps => ps.Vehicle == vehicle);

            logger.LogWarning("Vehicle might be parked.");

            if (parkingSpace != null)
            {
                logger.LogInformation("Unparking vehicle.");
                parkingSpace.Vehicle = null;
                await context.SaveChangesAsync();
            }

            context.Vehicles.Remove(vehicle);
            context.SaveChanges();

            logger.LogInformation("Vehicle deleted.");
        }

        public async Task ParkVehicle(Guid parkingSpaceGuid, string registration)
        {
            logger.LogInformation("Getting vehicle.");

            Vehicle vehicle = await context.Vehicles.Where(v => v.Registration == registration).FirstOrDefaultAsync();

            if (vehicle == null)
            {
                throw new NotFoundException($"The vehicles with the registration: {registration}, could not be found.");
            }

            logger.LogInformation("Getting parking space.");

            var result = await context.ParkingSpaces
                .Include(ps => ps.Vehicle)
                .FirstOrDefaultAsync(ps => ps.Vehicle == vehicle);

            if (result != null)
            {
                throw new VehicleAlreadyParkedException(vehicle.Registration);
            }

            logger.LogInformation("Parking vehicle.");

            ParkingSpace parkingSpace = await context.ParkingSpaces
                .Where(ps => ps.GUID == parkingSpaceGuid)
                .Include(ps => ps.Vehicle)
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

            logger.LogInformation("Vehicle parked.");
        }
        // I'm thinking that on the frontend the user knows where his vehicle is parked
        // so when unparking just send that info with no need of sending the registration
        public async Task UnparkVehicle(Guid parkingSpaceGuid)
        {
            logger.LogInformation("Getting parking space.");

            ParkingSpace parkingSpace = await context.ParkingSpaces
                .Where(ps => ps.GUID == parkingSpaceGuid)
                .Include(ps => ps.Vehicle)
                .FirstOrDefaultAsync();

            if (parkingSpace == null)
            {
                throw new NotFoundException($"The parking space with number: {parkingSpace.Number}, could not be found.");
            }

            logger.LogInformation("Unarking vehicle.");

            parkingSpace.Vehicle = null;
            await context.SaveChangesAsync();

            logger.LogInformation("Vehicle unparked.");
        }

        public async Task<List<VehicleDTO>> GetAll()
        {
            logger.LogInformation("Getting vehicles.");

            var vehicles = await context.Vehicles
                .Include(v => v.User)
                .OrderBy(v => v.User.Email)
                .ToListAsync();

            return mapper.Map<List<VehicleDTO>>(vehicles);
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
