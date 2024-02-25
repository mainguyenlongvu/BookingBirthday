using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using BookingBirthday.Server.Models;
using BookingBirthday.Application;
using BookingBirthday.Server.Common;
using BookingBirthday.Data.Enums;

namespace BookingBirthday.Server.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookingDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string _imageContentFolder;

        public AccountController(BookingDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
            _imageContentFolder = Path.Combine(webHostEnvironment.WebRootPath, "imgProfile");
        }

        // POST: Users/Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel loginData)
        {
            var pwt = CreateMD5.MD5Hash(loginData.Password!);
            var user = _context.Users.FirstOrDefault(x => x.Username == loginData.Username && x.Password == pwt);
            if (user != null)
            {
                HttpContext.Session.SetString("username", user.Username!);
                HttpContext.Session.SetString("role", user.Role!);
                HttpContext.Session.SetString("user_id", user.Id.ToString()!);
                HttpContext.Session.SetString("name", user.Name!);
                HttpContext.Session.SetString("email", user.Email!);
                HttpContext.Session.SetString("phone", user.Phone!);
                HttpContext.Session.SetString("address", user.Address!);
                if (user.Role == "Admin")
                {
                    TempData["Message"] = "Chào mừng quản trị viên";
                }
                else if (user.Role == "Host")
                {
                    TempData["Message"] = "Chào mừng chủ tiệc";
                }
                else
                {
                    TempData["Message"] = "Chào mừng khách hàng";
                }
                TempData["Success"] = true;
                return RedirectToAction("Index", "Home");
            }
            TempData["Message"] = "Đăng nhập không thành công";
            TempData["Success"] = false;
            return View(loginData);
        }
        
        // POST: Users/Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterModel userData)
        {
            try
            {
                if (userData.Password != userData.ConfirmPassword)
                {
                    TempData["Message"] = "Mật khẩu xác nhận không đúng";
                    TempData["Success"] = false;
                    return View(userData);
                }
                var usr = _context.Users.Where(x => x.Username == userData.Username || x.Email == userData.Email);
                if (usr.Count() > 0)
                {
                    TempData["Message"] = "Tài khoản đã tồn tại";
                    TempData["Success"] = false;
                    return View(userData);
                }
                var user = new User();
                user.Username = userData.Username!;
                user.Role = "Guest";
                user.Password = CreateMD5.MD5Hash(userData.Password!);
                user.Email = userData.Email!;
                user.Address = userData.Address;
                user.Phone = userData.Phone;
                user.Name = userData.Name;
                if (userData.File != null)
                {
                    user.Image_url = UploadedFile(userData.File);
                }
                else
                {
                    user.Image_url = "";
                }
                _context.Users.Add(user);
                _context.SaveChanges();
                TempData["Message"] = "Đăng kí thành công";
                TempData["Success"] = true;
                return RedirectToAction("Login", "Account");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Lỗi tạo tài khoản";
                TempData["Success"] = false;
                return View(userData);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Profile
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("user_id") != null)
            {
                var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
                var user = _context.Users.FirstOrDefault(x => x.Id == user_id);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                return View(user);
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public IActionResult UpdateProfile(RegisterModel userData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = int.Parse(HttpContext.Session.GetString("user_id") ?? "0");
                    var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                    if (user == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    // Update user properties
                    user.Name = userData.Name;
                    user.Email = userData.Email;
                    user.Address = userData.Address;
                    user.Phone = userData.Phone;

                    // Update password if provided and valid
                    if (!string.IsNullOrEmpty(userData.Password) && userData.Password == userData.ConfirmPassword)
                    {
                        user.Password = CreateMD5.MD5Hash(userData.Password);
                    }

                    // Update profile picture if provided
                    if (userData.File != null)
                    {
                        // Handle profile picture upload
                        user.Image_url = UploadedFile(userData.File);
                    }

                    _context.SaveChanges();

                    TempData["Message"] = "Cập nhật thông tin thành công";
                    TempData["Success"] = true;
                    return RedirectToAction("Profile", "Account");
                }
                catch (Exception e)
                {
                    TempData["Message"] = "Lỗi cập nhật thông tin tài khoản";
                    TempData["Success"] = false;
                    // Log the exception or handle it appropriately
                    return RedirectToAction("Profile", "Account");
                }
            }
            else
            {
                // If model validation fails, return to the profile edit page with error messages
                TempData["Message"] = "Vui lòng điền đúng thông tin";
                TempData["Success"] = false;
                return RedirectToAction("Profile", "Account");
            }
        }

        private string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null!;

            if (file != null)
            {
                string uploadsFolder = _imageContentFolder;
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return "/imgProfile/" + uniqueFileName!;
        }
        public void DeleteImage(string fileName)
        {
            var filePath = Path.Combine(_imageContentFolder, fileName);
            if (System.IO.File.Exists(filePath))
            {
                Task.Run(() => System.IO.File.Delete(filePath));
            }
        }
    }
}
