﻿using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Interfaces
{
    public interface IZoneRepository //: IRepository<Zone>
    {
        Task<ZoneDataDTO> Add(ZoneDTO request);
        Task AddArea(Guid zoneGuid, ParkingAreaDTO parkingAreaDTO);
        Task<List<ZoneDataDTO>> GetAll();
        Task<ZoneDataDTO> Update(ZoneDTO zoneDTO);
        Task Delete(ZoneDTO zoneDTO);
    }
}
