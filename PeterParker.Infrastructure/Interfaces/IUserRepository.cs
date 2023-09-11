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
        Task<string> LogInUser(UserLoginDTO request);
        Task<List<UserDataDTO>> GetAll();
        Task RegisterUser(UserRegisterDTO request);
        Task<UserDataDTO> ReturnUserData(HttpRequest request);
        Task AddAdminRole(UserLoginDTO request);
        Task RemoveAdminRole(UserLoginDTO request);
        Task RemoveInspectorRole(UserLoginDTO request);
        Task AddInspectorRole(UserLoginDTO request);
    }
}
