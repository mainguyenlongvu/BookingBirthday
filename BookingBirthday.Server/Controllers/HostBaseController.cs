using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookingBirthday.Server.Controllers
{
    public class HostBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = context.HttpContext.Session.GetString("username");
            var role = context.HttpContext.Session.GetString("role");
            if (username == null || role != "Host")
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
