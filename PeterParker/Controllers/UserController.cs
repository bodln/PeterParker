using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.Models;
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
        public async Task<IActionResult> RegisterUser(UserRegisterDTO request)
        {
            await unitOfWork.UserRepository.RegisterUser(request);
            return Ok(request.Email + " Successfully Registered.");
        }

        [HttpPost("LogIn")]
        public async Task<AuthTokens> LogInUser(UserLoginDTO request)
        {
            AuthTokens tokens = await unitOfWork.UserRepository.LogInUser(request);
            return tokens;
        }

        [HttpGet("Data")]
        [Authorize]
        public async Task<IActionResult> GetUserData()
        {
            UserDataDTO userDTO = await unitOfWork.UserRepository.ReturnUserData(HttpContext.Request);
            return Ok(userDTO);
        }

        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateUserData(UserRegisterDTO registerDTO)
        {
            await unitOfWork.UserRepository.Update(HttpContext.Request, registerDTO);
            return Ok();
        }

        [HttpPost("TokenRefresh")]
        public async Task<AuthTokens> TokenRefresh()
        {
            //string refreshToken = Request.Cookies["refresh-token"];
            string refreshToken = Request.Headers["X-Pp-Refresh-Token"].ToString();
            return await unitOfWork.UserRepository.TokenRefresh(refreshToken);
        }

        [HttpPost("MakeAdmin")]
        //[Authorize("AdminOnly")]
        public async Task<IActionResult> AddAdminRole(UserLoginDTO request)
        {
            await unitOfWork.UserRepository.AddAdminRole(request);
            return Ok("User Successfully Made Admin");
        }

        [HttpPost("RevokeAdmin")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> RemoveAdminRole(UserLoginDTO request)
        {
            await unitOfWork.UserRepository.RemoveAdminRole(request);
            return Ok("User Successfully Revoked as Admin");

        }

        [HttpPost("MakeInspector")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> AddInspectorRole(UserLoginDTO request)
        {
            await unitOfWork.UserRepository.AddInspectorRole(request);
            return Ok("User Successfully Made Inspector");
        }

        [HttpPost("RevokeInspector  ")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> RemoveInspectorRole(UserLoginDTO request)
        {
            await unitOfWork.UserRepository.RemoveInspectorRole(request);
            return Ok("User Successfully Revoked as Inspector");
        }

        [HttpGet("GetAllUsersWithVehicles")]
        public async Task<List<UserDataDTO>> GetAll()
        {
            var result = await unitOfWork.UserRepository.GetAll();
            return result;
        }
    }
}
