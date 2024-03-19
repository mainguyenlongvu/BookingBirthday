using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using X.PagedList;

namespace BookingBirthday.Server.Controllers
{
    public class AdminBookingController : AdminBaseController
    {
        private readonly BookingDbContext _dbContext;
        public AdminBookingController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(int? page)
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
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Booking> lst = new PagedList<Booking>(orders, pageNumber, pageSize);
            return View(lst);
        }
    }
}
