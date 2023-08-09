using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Interfaces
{
    public interface IVehicleRepository //: IRepository<VehicleDTO>
    {
        Task AddVehicle(VehicleDTO request);
        Task DeleteVehicle(string request);
        Task<List<VehicleDTO>> GetAllVehiclesForUserByEmail(string request);
        Task ParkVehicle(string registration, string zoneGeoJSON, int parkingSpaceNumber);
        Task UnparkVehicle(string zoneGeoJSON, int parkingSpaceNumber);
    }
}
