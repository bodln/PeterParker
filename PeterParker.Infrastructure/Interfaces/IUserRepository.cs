using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.DTOs;
using PeterParker.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<string> LogInUser(UserDTO request);
        Task<List<UserDTO>> GetAll();
        Task RegisterUser(UserDTO request);
        Task<UserDTO> ReturnUserData(HttpRequest request);
        Task AddAdminRole(string request);
        Task RemoveAdminRole(string request);
        Task RemoveInspectorRole(string request);
        Task AddInspectorRole(string request);
    }
}
