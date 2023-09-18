using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Interfaces
{
    public interface IParkingSpaceRepository //: IRepository<ParkingSpace>
    {
        Task<List<ParkingSpace>> AddParkingSpacesToArea(Guid zoneGuid, ParkingAreaDTO parkingAreaDTO);
        Task AddParkingSpaceToAreaByGuid(Guid request);
        Task<List<ParkingSpaceDTO>> GetAllByGeoJSON(string request);
        Task<List<ParkingSpaceDTO>> GetAllByGuid(Guid request);
    }
}
