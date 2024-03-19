using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using X.PagedList;

namespace BookingBirthday.Server.Controllers
{
    public class AdminReportController : AdminBaseController
    {
        private readonly BookingDbContext _dbContext;

        public AdminReportController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int? page)
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
                    .Where(x => x.category_name == null)
                    .OrderByDescending(x => x.created_at)
                    .ToListAsync();
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Category_requests> lst = new PagedList<Category_requests>(all_category_request, pageNumber, pageSize);

            return View(lst);
        }
        [HttpPost]
        public IActionResult Approved(int category_request_id, int is_approved, string? rejection_reason)
        {
            try
            {
                var category_Requests = _dbContext.Category_Requests.Find(category_request_id);
                if (category_Requests != null)
                {
                    
                    category_Requests.is_approved = is_approved;
                    if(rejection_reason != null)
                    {
                        category_Requests.rejection_reason = rejection_reason;

                    }
                    else 
                    {
                        category_Requests.rejection_reason = "";
                    }
                    _dbContext.SaveChanges();
                    TempData["Message"] = "Duyệt Khiếu nại thành công";
                    TempData["Success"] = true;
                }
                else
                {
                    TempData["Message"] = "Khiếu nại mới không tồn tại";
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
        public IActionResult Seen(int category_request_id)
        {
            var p = _dbContext.Category_Requests.SingleOrDefault(x => x.category_request_id == category_request_id);
            if (p == null)
            {
                TempData["Message"] = "Khiếu nại không tồn tại";
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
                TempData["Message"] = "Khiếu nại không tồn tại";
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
        public IActionResult getReport(int is_approved)
        {
            if (is_approved != 2)
            {
                var query = _dbContext.Category_Requests
                    .Where(x => x.is_approved == is_approved && x.report != null)
                    .OrderByDescending(x => x.created_at)
                    .ToList();

                return Json(query);
            }
            else
            {
                var query = _dbContext.Category_Requests.Where(x => x.report != null)
                    .OrderByDescending(x => x.created_at)
                    .ToList();

                return Json(query);
            }
        }
    }
}
