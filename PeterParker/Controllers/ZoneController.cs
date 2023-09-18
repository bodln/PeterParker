using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.Infrastructure;
using PeterParker.Infrastructure.Commands;

namespace PeterParker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public ZoneController(IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(ZoneDTO request)
        {
            var query = new AddZoneCommand()
            {
                request = request
            };
            return Ok(await mediator.Send(query));
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            List<ZoneDataDTO> zones = await unitOfWork.ZoneRepository.GetAll(); 
            return Ok(zones);
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> Update(ZoneDTO request)
        {
            var query = new UpdateZoneCommand()
            {
                request = request
            };
            return Ok(await mediator.Send(query));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(ZoneDTO request)
        {
            await unitOfWork.ZoneRepository.Delete(request);
            return Ok("Zone deleted");
        }

        [HttpPost("AddArea")]
        public async Task<IActionResult> AddArea(AddAreaDTO request)
        {
            await unitOfWork.ZoneRepository.AddArea(request.ZoneGUID, request.ParkingArea);
            return Ok("Area added.");
        }
    }
}
