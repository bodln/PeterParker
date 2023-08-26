using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeterParker.DTOs;
using PeterParker.Infrastructure;

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
            await unitOfWork.UserRepository.RegisterUser(request);
            return Ok(request.Email + " Successfully Registered.");
        }

        [HttpPost("LogIn")]
        public async Task<string> LogInUser(UserDTO request)
        {
            string token = await unitOfWork.UserRepository.LogInUser(request);
            return token;
        }

        [HttpPost("MakeAdmin")]
        //[Authorize("AdminOnly")]
        public async Task<IActionResult> AddAdminRole(string request)
        {
            await unitOfWork.UserRepository.AddAdminRole(request);
            return Ok("User Successfully Made Admin");
        }

        [HttpPost("RevokeAdmin")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> RemoveAdminRole(string request)
        {
            await unitOfWork.UserRepository.RemoveAdminRole(request);
            return Ok("User Successfully Revoked as Admin");

        }

        [HttpPost("MakeInstructor")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> AddInstructorRole(UserDTO request)
        {
            await unitOfWork.UserRepository.AddInstructorRole(request.Email);
            return Ok("User Successfully Made Instructor");
        }

        [HttpPost("RevokeInstructor")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> RemoveInstructorRole(UserDTO request)
        {
            await unitOfWork.UserRepository.RemoveInstructorRole(request.Email);
            return Ok("User Successfully Revoked as Instructor");
        }

        [HttpGet("GetAllUsersWithVehicles")]
        public async Task<List<UserDTO>> GetAll()
        {
            var result = await unitOfWork.UserRepository.GetAll();
            return result;
        }
    }
}
