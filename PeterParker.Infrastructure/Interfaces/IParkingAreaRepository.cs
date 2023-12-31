﻿using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Interfaces
{
    public interface IParkingAreaRepository //: IRepository<ParkingSpace>
    {
        Task<ParkingArea> CreateParkingArea(Guid zoneGuid, ParkingAreaDTO request);
        Task DeleteArea(ParkingAreaDTO parkingAreaDTO);
        Task<List<ParkingAreaDTO>> GetAllParkingAreas();
        Task<List<ParkingAreaDTO>> SearchAreas(string request);
    }
}
