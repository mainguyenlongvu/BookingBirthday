using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace BookingBirthday.Server.Controllers
{
    public class BookingController : BaseController
    {
        private readonly BookingDbContext _dbContext;

        public BookingController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult ViewOrders()
        {
            var orders = _dbContext.Bookings.OrderByDescending(x => x.Date_order).Where(x => x.Id == int.Parse(HttpContext.Session.GetString("user_id")!)).ToList();
            return View(orders);
        }
        public IActionResult ViewOrder(int orderId)
        {
            var session = HttpContext.Session;
            var role = HttpContext.Session.GetString("role");
            List<Category_requests> category_request = null!;

            if (role != null)
            {

                if (role == "Admin")
                {
                    category_request = _dbContext.Category_Requests
                        .Where(x => x.is_approved == 0 && x.is_deleted_by_admin == false)
                        .OrderByDescending(x => x.created_at)
                        .ToList();
                }
                else if (role == "Store Owner")
                {
                    var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
                    category_request = _dbContext.Category_Requests
                        .Where(x => x.is_deleted_by_owner == false && x.requester_id == user_id && (x.is_approved == -1 || x.is_approved == 1))
                        .OrderByDescending(x => x.created_at)
                        .ToList();
                }

                if (category_request != null)
                {
                    var jsonNotification = JsonConvert.SerializeObject(category_request);
                    session.SetString("notification", jsonNotification);
                }
            }


            var query = from a in _dbContext.Carts.Include(x => x.Package)
                        where a.BookingId == orderId
                        select new { a };
            if (query != null)
            {
                var data = query.Select(x => new CartModel()
                {
                    Id = x.a.Id,
                    BookingId = orderId,
                    PackageId = x.a.PackageId,
                    Package_Name = x.a.Package!.Name,
                    Price = x.a.Price,
                    Total = x.a.Total,
                }).ToList();
                return Json(data);
            }
            return Json(null);
        }
        public async Task<IActionResult> HuyDon(int orderId)
        {
            try
            {
                var data = await _dbContext.Bookings.SingleOrDefaultAsync(x => x.Id == orderId);
                if (data != null)
                {
                    data.BookingStatus = Data.Enums.BookingStatus.Declined;
                    await _dbContext.SaveChangesAsync();
                    var order_item = await _dbContext.Carts.Where(x => x.BookingId == orderId).ToListAsync();
                    if (order_item != null)
                    {
                        foreach (var item in order_item)
                        {
                            var product = await _dbContext.Packages.SingleOrDefaultAsync(x => x.Id == item.Id);
                            if (product != null)
                            {
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                    }
                    TempData["Message"] = "Hủy đơn hàng thành công";
                    TempData["Success"] = true;
                    return Json(data);
                }
                TempData["Message"] = "Đơn hàng không tồn tại";
                TempData["Success"] = false;
                return Json(null);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Lỗi";
                TempData["Success"] = false;
                return RedirectToAction("", "Cart");
            }

        }
    }
}
