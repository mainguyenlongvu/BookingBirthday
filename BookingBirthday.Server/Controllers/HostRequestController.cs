using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace BookingBirthday.Server.Controllers
{
    public class HostRequestController : HostBaseController
    {
        private readonly BookingDbContext _dbContext;

        public HostRequestController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
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

            var all_category_request = await _dbContext.Category_Requests
                    .OrderByDescending(x => x.created_at)
                    .ToListAsync();
            return View(all_category_request);
        }
        
        public IActionResult Seen(int category_request_id)
        {
            var host_name = HttpContext.Session.GetString("name")!;
            var p = _dbContext.Category_Requests.SingleOrDefault(x => x.category_request_id == category_request_id && x.host_name == host_name);
            if (p == null)
            {
                TempData["Message"] = "Yêu cầu mới không tồn tại hoặc không thuộc về bạn";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
            try
            {
                p.is_viewed_by_admin = true;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Lỗi";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult IsDelete(int category_request_id)
        {
            var host_name = HttpContext.Session.GetString("name")!;
            var p = _dbContext.Category_Requests.SingleOrDefault(x => x.category_request_id == category_request_id && x.host_name == host_name);
            if (p == null)
            {
                TempData["Message"] = "Yêu cầu mới không tồn tại hoặc không thuộc về bạn";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
            try
            {
                p.is_deleted_by_owner = true;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Lỗi";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult getRequest(int is_approved)
        {
            var host_name = HttpContext.Session.GetString("name")!;
            IQueryable<Category_requests> query = _dbContext.Category_Requests.Where(x => x.host_name == host_name);
        
        if (is_approved != 2)
        {
            query = query.Where(x => x.is_approved == is_approved);
        }

        var data = query
            .OrderByDescending(x => x.created_at)
            .ToList();

        return Json(data);
            }
        }
    }
}
