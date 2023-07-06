using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.DTOs;
using PeterParker.Infrastructure;
using PeterParker.Infrastructure.Interfaces;

namespace PeterParker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(UserDTO request)
        {
            IdentityResult result = unitOfWork.UserRepository.RegisterUser(request).Result;

            if (result.Succeeded)
            {
                return Ok(request.Email + " Successfully Registered.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("LogIn")]
        public async Task<string> LogInUser(UserDTO request)
        {
            return await unitOfWork.UserRepository.LogInUser(request);
        }

        [HttpPost("MakeAdmin")]
        //[Authorize("AdminOnly")]
        public async Task<IActionResult> AddAdminRole(string request)
        {
            IdentityResult result = unitOfWork.UserRepository.AddAdminRole(request).Result;

            if (result.Succeeded)
            {
                return Ok("User Successfully Made Admin");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("RevokeAdmin")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> RemoveAdminRole(string request)
        {
            IdentityResult result = unitOfWork.UserRepository.RemoveAdminRole(request).Result;

            if (result.Succeeded)
            {
                return Ok("User Successfully Revoked as Admin");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("MakeInstructor")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> AddInstructorRole(string request)
        {
            IdentityResult result = unitOfWork.UserRepository.AddInstructorRole(request).Result;

            if (result.Succeeded)
            {
                return Ok("User Successfully Made Instructor");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("RevokeInstructor")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> RemoveInstructorRole(string request)
        {
            IdentityResult result = unitOfWork.UserRepository.RemoveInstructorRole(request).Result;

            if (result.Succeeded)
            {
                return Ok("User Successfully Revoked as Instructor");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("GetAllUsersWithVehicles")]
        public async Task<List<UserDTO>> GetAll()
        {
            return await unitOfWork.UserRepository.GetAll();

        }
    }
}
