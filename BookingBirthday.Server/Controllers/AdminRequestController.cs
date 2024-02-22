using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace BookingBirthday.Server.Controllers
{
    public class AdminRequestController : AdminBaseController
    {
        private readonly BookingDbContext _dbContext;

        public AdminRequestController(BookingDbContext dbContext)
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
        //[HttpPost]
        //public IActionResult Approved(int category_request_id, int is_approved, string? rejection_reason)
        //{
        //    try
        //    {
        //        var category_Requests = _dbContext.Category_Requests.Find(category_request_id);
        //        if (category_Requests != null)
        //        {
        //            if (is_approved == 1)
        //            {
        //                var category = new Categories();
        //                category.name = category_Requests.category_name;
        //                _dbContext.Categories.Add(category);
        //            }
        //            category_Requests.is_approved = is_approved;
        //            category_Requests.rejection_reason = rejection_reason;
        //            _dbContext.SaveChanges();
        //            TempData["Message"] = "Duyệt yêu cầu thêm mới danh mục thành công";
        //            TempData["Success"] = true;
        //        }
        //        else
        //        {
        //            TempData["Message"] = "Yêu cầu thêm mới danh mục không tồn tại";
        //            TempData["Success"] = false;
        //        }
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Message"] = "Lỗi";
        //        TempData["Success"] = false;
        //        return BadRequest();
        //    }
        //}
        public IActionResult Seen(int category_request_id)
        {
            var p = _dbContext.Category_Requests.SingleOrDefault(x => x.category_request_id == category_request_id);
            if (p == null)
            {
                TempData["Message"] = "Yêu cầu thêm mới danh mục không tồn tại";
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
            var p = _dbContext.Category_Requests.SingleOrDefault(x => x.category_request_id == category_request_id);
            if (p == null)
            {
                TempData["Message"] = "Yêu cầu thêm mới danh mục không tồn tại";
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
            if (is_approved != 2)
            {
                var query = _dbContext.Category_Requests
                    .Where(x => x.is_approved == is_approved)
                    .OrderByDescending(x => x.created_at)
                    .ToList();

                return Json(query);
            }
            else
            {

                var query = _dbContext.Category_Requests
                    .OrderByDescending(x => x.created_at)
                    .ToList();

                return Json(query);
            }
        }
    }
}
