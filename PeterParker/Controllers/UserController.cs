using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                return Ok(request.Email + "Successfully Registered.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("LogIn")]
        public async Task<string> LogInUser(UserDTO request)
        {
            return await unitOfWork.UserRepository.LogInUser(request);
        }

        [HttpPost("MakeAdmin")]
        [Authorize("AdminOnly")]
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
    }
}
