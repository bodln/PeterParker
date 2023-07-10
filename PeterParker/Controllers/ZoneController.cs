using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.Infrastructure;

namespace PeterParker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ZoneController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("AddZone")]
        public async Task<IActionResult> Add(ZoneDTO request)
        {
            if (unitOfWork.ZoneRepository.Add(request))
            {
                unitOfWork.SaveChanges();
                return Ok("Zone added");
            }

            return BadRequest("Ooops!");
        }

        [HttpGet("GetAllZones")]
        public async Task<IActionResult> GetAll()
        {
            List<Zone> zones = unitOfWork.ZoneRepository.GetAll(); 
            if (zones != null)
            {
                return Ok(zones);
            }

            return BadRequest("Ooops!");
        }
    }
}
