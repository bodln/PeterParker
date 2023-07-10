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
        Task<bool> AddVehicle(VehicleDTO request);
        Task<bool> DeleteVehicle(string request);
        Task<List<VehicleDTO>> GetAllVehiclesForUserByEmail(string request);
        bool ParkVehicle(string registration, string zoneGeoJSON, int parkingSpaceNumber);
        bool UnparkVehicle(string zoneGeoJSON, int parkingSpaceNumber);
    }
}
