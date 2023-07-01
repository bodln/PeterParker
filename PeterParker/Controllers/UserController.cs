using Microsoft.AspNetCore.Mvc;

namespace PeterParker.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
