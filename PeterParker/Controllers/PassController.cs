using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
using PeterParker.Infrastructure;

namespace PeterParker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PassController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(PassCreationDTO zone)
        {
            PassDTO response = await unitOfWork.PassRepository.Add(HttpContext.Request, zone);
            return Ok(response);
        }
    }
}
