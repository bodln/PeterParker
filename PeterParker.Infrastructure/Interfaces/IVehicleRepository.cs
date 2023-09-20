using Microsoft.AspNetCore.Http;
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
        Task AddVehicle(HttpRequest request, VehicleDTO vehicleDTO);
        Task DeleteVehicle(VehicleDTO request);
        Task<List<VehicleDTO>> GetAllVehiclesForUserByEmail(HttpRequest request);
        Task ParkVehicle(ParkVehicleDTO parkVehicleDTO);
        Task UnparkVehicle(ParkVehicleDTO parkVehicleDTO);
    }
}
