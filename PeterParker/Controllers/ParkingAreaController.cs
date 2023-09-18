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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllParkingAreas()
        {
            var result = await unitOfWork.ParkingAreaRepository.GetAllParkingAreas();
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(ParkingAreaDTO request)
        {
            await unitOfWork.ParkingAreaRepository.DeleteArea(request);
            return Ok("Area deleted.");
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string request)
        {
            List<ParkingAreaDTO> response = await unitOfWork.ParkingAreaRepository.SearchAreas(request);
            return Ok(response);
        }
    }
}
