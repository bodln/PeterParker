using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
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
        public IActionResult GetAll()
        {
            var tickets = unitOfWork.TicketRepository.GetAll();
            return Ok(tickets);
        }

        [HttpPost("AddTicket")]
        public IActionResult Add(TicketDTO request)
        {
            unitOfWork.TicketRepository.Add(request);
            return Ok("Ticket added");
        }
    }
}
