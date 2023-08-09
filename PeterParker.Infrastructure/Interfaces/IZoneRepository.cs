using PeterParker.Data.DTOs;
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
        Task Add(ZoneDTO request);
        Task<List<Zone>> GetAll();
    }
}
