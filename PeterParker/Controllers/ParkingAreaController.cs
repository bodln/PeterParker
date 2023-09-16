using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
using PeterParker.Infrastructure;

namespace PeterParker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingAreaController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ParkingAreaController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddParkingArea(ParkingAreaDTO request)
        {
            await unitOfWork.ParkingAreaRepository.AddParkingArea(request);
            return Ok("Parking area added.");
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllParkingAreas()
        {
            var result = await unitOfWork.ParkingAreaRepository.GetAllParkingAreas();
            return Ok(result);
        }
    }
}
