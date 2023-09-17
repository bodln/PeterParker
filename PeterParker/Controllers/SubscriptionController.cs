using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Data.DTOs;
using PeterParker.Infrastructure;

namespace PeterParker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public SubscriptionController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(HttpRequest request)
        {
            SubscriptionDTO result = await unitOfWork.SubscriptionRepository.Get(request);
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(HttpRequest request)
        {
            await unitOfWork.SubscriptionRepository.Add(request);
            return Ok("Sunscription added.");
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(SubscriptionDTO request)
        {
            await unitOfWork.SubscriptionRepository.Delete(request);
            return Ok("Subscription deleted.");
        }
    }
}
