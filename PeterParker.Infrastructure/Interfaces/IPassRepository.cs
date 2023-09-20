using Microsoft.AspNetCore.Http;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Interfaces
{
    public interface IPassRepository
    {
        Task<PassDTO> Add(HttpRequest request, PassCreationDTO passCreationDTO);
    }
}
