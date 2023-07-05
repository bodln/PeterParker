using Microsoft.AspNetCore.Identity;
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
        Task<string> LogInUser(UserDTO request);
        Task<IdentityResult> RegisterUser(UserDTO request);
    }
}
