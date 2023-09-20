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

        [HttpPost("Subscribe")]
        public async Task<IActionResult> Subscribe(SubscriptionDTO subscription)
        {
            await unitOfWork.SubscriptionRepository.Add(HttpContext.Request, subscription);
            return Ok("Subscription added.");
        }

        [HttpGet("GetPrices")]
        public object GetPrices()
        {
            object prices = unitOfWork.SubscriptionRepository.Prices();
            return prices;
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(SubscriptionDTO request)
        {
            await unitOfWork.SubscriptionRepository.Delete(request);
            return Ok("Subscription deleted.");
        }
    }
}
