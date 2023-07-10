using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.Infrastructure;

namespace PeterParker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public TicketController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllTickets")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(unitOfWork.TicketRepository.GetAll());
        }

        [HttpPost("AddTicket")]
        public async Task<IActionResult> Add(TicketDTO request)
        {
            try
            {
                unitOfWork.TicketRepository.Add(request);
                return Ok("Ticket added");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
