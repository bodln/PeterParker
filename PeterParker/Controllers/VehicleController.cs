using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
using PeterParker.Infrastructure;

namespace PeterParker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public VehicleController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("AddVehicle")]
        public IActionResult AddVehicle(VehicleDTO request)
        {
            if ((unitOfWork.VehicleRepository.AddVehicle(request)).Result)
            {
                unitOfWork.SaveChanges();
                return Ok("Vehicle successfully added.");
            }
            return BadRequest("Something went wrong.");
        }

        [HttpDelete("DeleteVehicle")]
        public IActionResult DeleteVehicle(string request)
        {
            if (unitOfWork.VehicleRepository.DeleteVehicle(request).Result)
            {
                unitOfWork.SaveChanges();
                return Ok("Vehicle successfully deleted.");
            }

            return BadRequest("Something went wrong.");
            
        }

        [HttpGet("GetAllVehiclesByEmail")]
        public async Task<List<VehicleDTO>> GetAllVehiclesByEmail(string request)
        {
            return await unitOfWork.VehicleRepository.GetAllVehiclesForUserByEmail(request);
        }
    }
}
