using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace BookingBirthday.Server.Controllers
{
    public class AdminBookingController : AdminBaseController
    {
        private readonly BookingDbContext _dbContext;

        public AdminBookingController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var session = HttpContext.Session;
            List<Category_requests> category_request = null!;

            category_request = _dbContext.Category_Requests
                    .Where(x => x.is_approved == 0 && x.is_deleted_by_admin == false)
                    .OrderByDescending(x => x.created_at)
                    .ToList();

            if (category_request != null)
            {
                var jsonNotification = JsonConvert.SerializeObject(category_request);
                session.SetString("notification", jsonNotification);
            }

            var orders = _dbContext.Bookings.OrderByDescending(x => x.Date_order).ToList();
            return View(orders);
        }
    }
}
