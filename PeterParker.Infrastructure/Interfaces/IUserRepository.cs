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
        Task AddAdminRole(string request);
        Task RemoveAdminRole(string request);
        Task RemoveInstructorRole(string request);
        Task AddInstructorRole(string request);
    }
}
