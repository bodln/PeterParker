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
        Task<IdentityResult> AddAdminRole(string request);
        Task<IdentityResult> AddInstructorRole(string request);
        Task<string> LogInUser(UserDTO request);
        Task<IdentityResult> RegisterUser(UserDTO request);
        Task<IdentityResult> RemoveAdminRole(string request);
        Task<IdentityResult> RemoveInstructorRole(string request);
        Task<List<UserDTO>> GetAll();
    }
}
