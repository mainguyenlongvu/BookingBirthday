using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Common;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace BookingBirthday.Server.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly BookingDbContext _dbContext;

        public AdminUserController(BookingDbContext dbContext)
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

            var users = _dbContext.Users.OrderByDescending(x => x.Id).ToList();
            return View(users);
        }
        [HttpPost]
        public IActionResult Create(RegisterModel userData)
        {
            try
            {
                if (userData.Password != userData.ConfirmPassword)
                {
                    TempData["Message"] = "Mật khẩu xác nhận không đúng";
                    TempData["Success"] = false;
                    return RedirectToAction("Index");
                }
                var usr = _dbContext.Users.Where(x => x.Username == userData.Username || x.Email == userData.Email);
                if (usr.Count() > 0)
                {
                    TempData["Message"] = "Tài khoản đã tồn tại";
                    TempData["Success"] = false;
                    return RedirectToAction("Index");
                }
                var user = new Data.Entities.User();
                user.Username = userData.Username;
                user.Role= "Guest";
                user.Password = CreateMD5.MD5Hash(userData.Password!);
                user.Email = userData.Email;
                user.Address = userData.Address;
                user.Phone = userData.Phone;
                user.Name = userData.Name;
                user.Image_url = "";
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                TempData["Message"] = "Thêm mới người dùng thành công";
                TempData["Success"] = true;
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Lỗi tạo tài khoản";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Edit(RegisterModel userData)
        {
            try
            {
                if (userData.Password != userData.ConfirmPassword)
                {
                    TempData["Message"] = "Mật khẩu xác nhận không đúng";
                    TempData["Success"] = false;
                    return RedirectToAction("Index");
                }
                var user = _dbContext.Users.SingleOrDefault(x => x.Id == userData.User_id);
                if (user == null)
                {
                    TempData["Message"] = "Tài khoản không tồn tại";
                    TempData["Success"] = false;
                    return View(userData);
                }
                if (user.Email != userData.Email)
                {
                    var usr = _dbContext.Users.Where(x => x.Email == userData.Email);
                    if (usr.Count() > 0)
                    {
                        TempData["Message"] = "Tài khoản đã tồn tại";
                        TempData["Success"] = false;
                        return RedirectToAction("Index");
                    }
                }
                if (user.Username != userData.Username)
                {
                    var usr = _dbContext.Users.Where(x => x.Username == userData.Username);
                    if (usr.Count() > 0)
                    {
                        TempData["Message"] = "Tài khoản đã tồn tại";
                        TempData["Success"] = false;
                        return RedirectToAction("Index");
                    }
                }
                user.Role = userData.Role;
                if (userData.Password != null)
                {
                    user.Password = CreateMD5.MD5Hash(userData.Password!);
                }
                user.Email = userData.Email;
                user.Address = userData.Address;
                user.Phone = userData.Phone;
                user.Name = userData.Name;
                _dbContext.SaveChanges();
                TempData["Message"] = "Chỉnh sửa thông tin người dùng thành công";
                TempData["Success"] = true;
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Lỗi";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Delete(int userId)
        {
            try
            {
                var user = _dbContext.Users.Find(userId);
                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                    _dbContext.SaveChanges();
                    TempData["Message"] = "Xóa tài khoản người dùng thành công";
                    TempData["Success"] = true;
                }
                else
                {
                    TempData["Message"] = "Xóa tài khoản người dùng không thành công";
                    TempData["Success"] = false;
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Lỗi";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
        }
    }
}
