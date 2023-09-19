﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PeterParker.Data;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.DTOs;
using PeterParker.Infrastructure.Exceptions;
using PeterParker.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
        public async Task AddVehicle(HttpRequest request, VehicleDTO vehicleDTO)
        {
            if (await context.Vehicles.AnyAsync(v => v.Registration == vehicleDTO.Registration))
            {
                throw new DuplicateObjectException($"A vehicle with the registration: {vehicleDTO.Registration}, already exists.");
            }

            Vehicle vehicle = mapper.Map<Vehicle>(vehicleDTO);

            string token = request.Headers["Authorization"].ToString().Replace("bearer ", "");

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            string email = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;

            vehicle.User = await context.Users
                .Where(u => u.Email == email)
                .Include(u => u.Subscription)
                .Include(u => u.Tickets)
                .Include(u => u.Pass)
                .FirstOrDefaultAsync();

            vehicle.GUID = Guid.NewGuid();

            if (vehicle.User == null)
            {
                throw new NotFoundException("The owner for this vehicle has not been found.");
            }

            logger.LogInformation("Adding vehicle.");

            context.Vehicles.Add(vehicle);
            context.SaveChanges();

            logger.LogInformation("Vehicle added.");
        }

        public async Task<List<VehicleDTO>> GetAllVehiclesForUserByEmail(HttpRequest request)
        {
            logger.LogInformation("Getting vehicle owner.");

            string token = request.Headers["Authorization"].ToString().Replace("bearer ", "");

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            string email = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;

            User user = await context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new NotFoundException("The owner of the vehicles could not be found.");
            }

            logger.LogInformation("Owner found.");
            logger.LogInformation("Getting vehicles.");

            List<Vehicle> vehicles = await context.Vehicles.Where(v => v.User == user).ToListAsync();

            if (vehicles == null)
            {
                logger.LogInformation("Vehicles found.");
            }

            List<VehicleDTO> vehiclesDTO = mapper.Map<List<VehicleDTO>>(vehicles);

            return vehiclesDTO;
        }

        public async Task DeleteVehicle(VehicleDTO request)
        {
            logger.LogInformation("Getting vehicles.");

            Vehicle vehicle = await context.Vehicles
                .FirstOrDefaultAsync(v => v.Registration == request.Registration);

            if (vehicle == null)
            {
                throw new NotFoundException($"The vehicles with the registration: {request}, could not be found.");
            }

            logger.LogInformation("Vehicle found.");

            if (vehicle.ParkingSpaceGuid != Guid.Empty)
            {
                ParkingSpace parkingSpace = await context.ParkingSpaces
                    .Include(ps => ps.Vehicle)
                .FirstOrDefaultAsync(ps => ps.GUID == request.GUID);

                logger.LogInformation("Unparking vehicle.");

                parkingSpace.Vehicle = null;
                await context.SaveChangesAsync();
            }

            context.Vehicles.Remove(vehicle);
            context.SaveChanges();

            logger.LogInformation("Vehicle deleted.");
        }

        public async Task ParkVehicle(ParkVehicleDTO parkVehicleDTO)
        {
            ParkingSpace parkingSpace = await context.ParkingSpaces
                .Where(ps => ps.GUID == parkVehicleDTO.ParkingSpaceGuid)
                .Include(ps => ps.Vehicle)
                .FirstOrDefaultAsync();

            ParkingArea parkingArea = await context.ParkingAreas
                .Include(pa => pa.ParkingSpaces)
                .Where(pa => pa.ParkingSpaces.Contains(parkingSpace))
                .FirstOrDefaultAsync();

            string[] workingHoursArray = parkingArea.WorkingHours.Split("-");

            TimeSpan start = TimeSpan.Parse(workingHoursArray[0]+":00");
            TimeSpan end = TimeSpan.Parse(workingHoursArray[1] + ":00"); 
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (start == end)
            {
                await ParkVehicleFunction(parkVehicleDTO, parkingSpace);
                return;
            }

            if (start < end)
            {
                // start and stop times are in the same day
                if (now >= start && now <= end)
                {
                    await ParkVehicleFunction(parkVehicleDTO, parkingSpace);
                    return;
                }
            }
            else
            {
                // start and stop times are in different days
                if (now >= start || now <= end)
                {
                    await ParkVehicleFunction(parkVehicleDTO, parkingSpace);
                    return;
                }
            }
                throw new AreaClosedException($"This parking area is currently close, and will remain so until {workingHoursArray[0]}:00");
        }

        private async Task ParkVehicleFunction(ParkVehicleDTO parkVehicleDTO, ParkingSpace parkingSpace)
        {
            logger.LogInformation("Getting vehicle.");

            Vehicle vehicle = await context.Vehicles.Where(v => v.GUID == parkVehicleDTO.VehicleGuid).FirstOrDefaultAsync();

            if (vehicle == null)
            {
                throw new NotFoundException($"The vehicles with the Guid: {parkVehicleDTO.VehicleGuid}, could not be found.");
            }
            
            vehicle.ParkingSpaceGuid = parkVehicleDTO.ParkingSpaceGuid;

            logger.LogInformation("Getting parking space.");

            var result = await context.ParkingSpaces
                .Include(ps => ps.Vehicle)
                .FirstOrDefaultAsync(ps => ps.Vehicle == vehicle);

            if (result != null)
            {
                throw new VehicleAlreadyParkedException(vehicle.Registration);
            }

            logger.LogInformation("Parking vehicle.");

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
                .Where(ps => ps.GUID == parkingSpaceGuid).Include(ps => ps.Vehicle).FirstOrDefaultAsync();

            if (parkingSpace == null)
            {
                throw new NotFoundException($"The parking space with number: {parkingSpace.Number}, could not be found.");
            }

            if (parkingSpace.Vehicle == null)
            {
                throw new NotFoundException($"There is no vehicle parked at this parking space.");
            }

            logger.LogInformation("Unarking vehicle.");

            parkingSpace.Vehicle.ParkingSpaceGuid = Guid.Empty;
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
