using Microsoft.AspNetCore.Mvc;

namespace BookingBirthday.Server.Controllers
{
    public class AdminBaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
