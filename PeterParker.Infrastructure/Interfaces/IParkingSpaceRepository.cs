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
        Task<List<ParkingSpaceDTO>> GetAllByGeoJSON(string request);
    }
}
