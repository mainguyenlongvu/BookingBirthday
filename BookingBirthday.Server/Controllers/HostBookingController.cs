using BookingBirthday.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static NuGet.Packaging.PackagingConstants;
using System;
using BookingBirthday.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace BookingBirthday.Server.Controllers
{
    public class HostBookingController : HostBaseController
    {
        private readonly BookingDbContext _dbContext;

        public HostBookingController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);

            var session = HttpContext.Session;
            List<Category_requests> category_request = null!;

            category_request = _dbContext.Category_Requests
                    .Where(x => x.is_deleted_by_owner == false && x.requester_id == user_id && (x.is_approved == -1 || x.is_approved == 1))
                    .OrderByDescending(x => x.created_at)
                    .ToList();

            if (category_request != null)
            {
                var jsonNotification = JsonConvert.SerializeObject(category_request);
                session.SetString("notification", jsonNotification);
            }

            var query = from a in _dbContext.Bookings
                        join b in _dbContext.Carts
                        on a.Id equals b.Id
                        join c in _dbContext.Packages
                        on b.Id equals c.Id
            where c.Id == user_id
                        select new { a };
            var orders = await query.OrderByDescending(x => x.a.DateOrder).Select(x => new Booking
            {
                Id = x.a.Id,
                UserId = x.a.UserId,
                Phone = x.a.Phone,
                Email = x.a.Email,
                Note = x.a.Note,
                DateOrder = x.a.DateOrder,
                Total = x.a.Total,
                BookingStatus = x.a.BookingStatus,

            }).ToListAsync();
            return View(orders);
        }
        [HttpPost]
        public IActionResult Edit(int orderId, int status)
        {
            try
            {
                var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);

                var order = _dbContext.Bookings.Find(orderId);
                if (order != null)
                {
                    order.BookingStatus = (Data.Enums.BookingStatus)status;
                    _dbContext.SaveChanges();
                    TempData["Message"] = "Cập nhật trạng thái đơn hàng thành công";
                    TempData["Success"] = true;
                }
                else
                {
                    TempData["Message"] = "Đơn hàng không tồn tại";
                    TempData["Success"] = false;
                }
                return Ok();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Lỗi";
                TempData["Success"] = false;
                return BadRequest();
            }
        }
        public IActionResult ProfileCustomer(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                TempData["Message"] = "Tài khoản không tồn tại";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}
