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
        public async Task<IActionResult> AddVehicle(VehicleDTO request)
        {
            await unitOfWork.VehicleRepository.AddVehicle(request);
            return Ok("Vehicle successfully added.");
        }

        [HttpDelete("DeleteVehicle")]
        public async Task<IActionResult> DeleteVehicle(VehicleDTO request)
        {
            await unitOfWork.VehicleRepository.DeleteVehicle(request);
            return Ok("Vehicle successfully deleted.");
        }

        [HttpGet("GetAllVehiclesByEmail")]
        public async Task<IActionResult> GetAllVehiclesByEmail(HttpRequest request)
        {
            var result = await unitOfWork.VehicleRepository.GetAllVehiclesForUserByEmail(request);
            return Ok(result);
        }

        [HttpPost("ParkVehicle")]
        public async Task<IActionResult> ParkVehicle(Guid parkingSpace, VehicleDTO vehicle)
        {
            await unitOfWork.VehicleRepository.ParkVehicle(parkingSpace, vehicle);
            return Ok("Successfully parked!");
        }

        [HttpPost("UnparkVehicle")]
        public async Task<IActionResult> UnparkVehicle(Guid request)
        {
            await unitOfWork.VehicleRepository.UnparkVehicle(request);
            return Ok("Successfully unparked!");
        }
    }
}
