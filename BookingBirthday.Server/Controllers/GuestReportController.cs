using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace BookingBirthday.Server.Controllers
{
    public class GuestReportController : GuestBaseController
    {
        private readonly BookingDbContext _dbContext;

        public GuestReportController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);

            var session = HttpContext.Session;
            List<Category_requests> category_request = null!;

            category_request = await _dbContext.Category_Requests
                    .Where(x => x.is_deleted_by_owner == false && x.requester_id == user_id && (x.is_approved == -1 || x.is_approved == 1))
                    .OrderByDescending(x => x.created_at)
                    .ToListAsync();


            if (category_request != null)
            {
                var jsonNotification = JsonConvert.SerializeObject(category_request);
                session.SetString("notification", jsonNotification);
            }

            var all_category_request = await _dbContext.Category_Requests
                    .Where(x => x.requester_id == user_id && x.category_name == null)
                    .OrderByDescending(x => x.created_at)
                    .ToListAsync();
            return View(all_category_request);
        }
        [HttpPost]
        public IActionResult Create(Category_requests request)
        {
            var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
            var categoryRequest = _dbContext.Category_Requests.SingleOrDefault(x => x.report == request.report);
            if (categoryRequest != null)
            {
                TempData["Message"] = "Khiếu nại đã tồn tại";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
            try
            {
                var p = new Category_requests();
                p.requester_id = user_id;
                p.category_name = request.category_name;
                p.report = request.report;
                p.created_at = DateTime.Now;
                p.is_approved = 0;
                p.host_name = request.host_name;
                p.guest_name = HttpContext.Session.GetString("name")!;
                p.is_deleted_by_admin = false;
                p.is_deleted_by_owner = false;
                p.is_viewed_by_admin = false;
                p.is_viewed_by_owner = false;
                p.rejection_reason = "";


                //rằng điều kiện có tên của Host vào đây

                //
                var userExists = _dbContext.Users.Any(x => x.Name == p.host_name);

                if (userExists)
                {
                    _dbContext.Category_Requests.Add(p);
                    _dbContext.SaveChanges();
                    TempData["Message"] = "Gửi khiếu nại mới thành công";
                    TempData["Success"] = true;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "không có tên chủ tiệc này!";
                    TempData["Success"] = false;
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Lỗi";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Edit(Category_requests request)
        {
            var p = _dbContext.Category_Requests.SingleOrDefault(x => x.category_request_id == request.category_request_id);
            if (p == null)
            {
                TempData["Message"] = "Khiếu nại mới không tồn tại";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
            try
            {
                p.report = request.report;
                p.host_name = request.host_name;
                _dbContext.SaveChanges();
                TempData["Message"] = "Chỉnh sửa Khiếu nại mới thành công";
                TempData["Success"] = true;
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
        public IActionResult Delete(int category_request_id)
        {
            try
            {
                var category_Requests = _dbContext.Category_Requests.Find(category_request_id);
                if (category_Requests != null)
                {

                    _dbContext.Category_Requests.Remove(category_Requests);
                    _dbContext.SaveChanges();
                    TempData["Message"] = "Xóa Khiếu nại mới thành công";
                    TempData["Success"] = true;
                }
                else
                {
                    TempData["Message"] = "Xóa Khiếu nại mới không thành công";
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
                p.is_viewed_by_owner = true;
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
            var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
            var query = _dbContext.Category_Requests
            .Where(x => x.requester_id == user_id && x.report != null);

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
