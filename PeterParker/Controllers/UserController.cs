using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeterParker.Infrastructure;
using PeterParker.Infrastructure.Interfaces;

namespace PeterParker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
