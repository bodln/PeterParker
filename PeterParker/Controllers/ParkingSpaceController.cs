using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
using PeterParker.Infrastructure;

namespace PeterParker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingSpaceController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ParkingSpaceController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllByGeoJSON")]
        public async Task<IActionResult> GetAllByGeoJSON(string request)
        {
            var result = await unitOfWork.ParkingSpaceRepository.GetAllByGeoJSON(request);

            return Ok(result);
        }
    }
}
