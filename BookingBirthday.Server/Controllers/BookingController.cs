using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace BookingBirthday.Server.Controllers
{
    public class BookingController : GuestBaseController
    {
        private readonly BookingDbContext _dbContext;

        public BookingController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult ViewBookings()
        {

            //Ràng điều kiện ở đây

            var orders = _dbContext.Bookings.OrderByDescending(x => x.Date_order).Where(x => x.UserId == int.Parse(HttpContext.Session.GetString("user_id")!)).ToList();
            return View(orders);
        }
        public IActionResult ViewBooking(int Id)
        {
            var session = HttpContext.Session;
            List<Category_requests> category_request = null!;
            var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);

            if (category_request != null)
                {
                    var jsonNotification = JsonConvert.SerializeObject(category_request);
                    session.SetString("notification", jsonNotification);
                }
            //}


            var query = from a in _dbContext.BookingPackages
                        join b in _dbContext.Bookings
                        on a.BookingId equals b.Id
                        where b.UserId == user_id && a.BookingId == Id
                        select new { a };
            if (query != null)
            {
                var data = query.Select(x => new BookingPackageModel()
                {
                    Booking_Package_Id = x.a.Id,
                    Booking_id = Id,
                    Package_Id = x.a.PackageId,
                    Package_name = x.a.Package!.Name,
                    price = x.a.Price,
                }).ToList();
                return Json (data);
            }
            return Json(null);
        }
        [HttpPost]
        public IActionResult HuyDon(int orderId)
        {
            try
            {
                var data =  _dbContext.Bookings.Find(orderId);
                if (data != null)
                {
                    data.BookingStatus = "Declined";
                    data.Date_cancel = DateTime.Now;
                     _dbContext.SaveChanges();
                    TempData["Message"] = "Nhân viên sẽ liên hệ hoàn cọc nếu bạn hủy đúng thời gian quy định";
                    TempData["Success"] = true;
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
    }
}
