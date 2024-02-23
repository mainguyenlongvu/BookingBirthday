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
                HttpContext.Session.SetString("role", user.Role!.ToString());
                HttpContext.Session.SetString("user_id", user.Id.ToString()!);
                HttpContext.Session.SetString("name", user.Name!);
                HttpContext.Session.SetString("email", user.Email!);
                HttpContext.Session.SetString("phone", user.Phone!);
                HttpContext.Session.SetString("address", user.Address!);
                if (user.Role.ToString() == "Admin")
                {
                    TempData["Message"] = "Welcome Admin";
                }
                else if (user.Role.ToString() == "Host")
                {
                    TempData["Message"] = "Welcome Host";
                }
                else
                {
                    TempData["Message"] = "Welcome Guest";
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
                user.Username = userData.Username;
                user.Role = Role.Guest;
                user.Password = CreateMD5.MD5Hash(userData.Password!);
                user.Email = userData.Email;
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
                    return RedirectToAction("Login", "Users");
                }
                return View(user);
            }
            return RedirectToAction("Login", "Users");
        }
        [HttpPost]
        public IActionResult UpdateProfile(RegisterModel userData)
        {
            try
            {
                if (HttpContext.Session.GetString("user_id") != null)
                {

                    var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
                    var email = HttpContext.Session.GetString("email");

                    if (email != userData.Email)
                    {
                        var usr = _context.Users.Where(x => x.Email == userData.Email);
                        if (usr.Count() > 0)
                        {
                            TempData["Message"] = "Tài khoản đã tồn tại";
                            TempData["Success"] = false;
                            return RedirectToAction("Profile", "Users");
                        }
                    }
                    var user = _context.Users.FirstOrDefault(x => x.Id == user_id);
                    if (user == null)
                    {
                        return RedirectToAction("Login", "Users");
                    }
                    else if (userData.Password != userData.ConfirmPassword)
                    {
                        TempData["Message"] = "Mật khẩu xác nhận không đúng";
                        TempData["Success"] = false;
                        return RedirectToAction("Index");
                    }
                    else if (userData.Password != null)
                    {
                        if (user.Password!.Length >= 8)
                        {
                            user.Password = CreateMD5.MD5Hash(userData.Password!);
                        }
                        else
                        {
                            TempData["Message"] = "Mật khẩu phải ít nhất 8 kí tự";
                            TempData["Success"] = false;
                            return RedirectToAction("Index");
                        }
                    }
                    user.Email = userData.Email;
                    user.Email = userData.Email;
                    user.Email = userData.Email;
                    user.Email = userData.Email;
                    if (userData.File != null)
                    {
                        if (user.Image_url != "/imageProfile/" && user.Image_url != null)
                        {
                            var n = user.Image_url!.Remove(0, 12);
                            DeleteImage(n);
                        }
                        user.Image_url = UploadedFile(userData.File);
                    }
                    _context.SaveChanges();
                    TempData["Message"] = "Cập nhật thông tin thành công";
                    TempData["Success"] = true;
                    return RedirectToAction("Profile", "Users");
                }

                return RedirectToAction("Login", "Users");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Lỗi cập nhật thông tin tài khoản";
                TempData["Success"] = false;
                return RedirectToAction("Index");
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

        //// GET: Users
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Users.ToListAsync());
        //}

        //// GET: Users/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// GET: Users/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Users/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Username,Password,Email,Role")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        //// GET: Users/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Users/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Email,Role")] User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(user);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        //// GET: Users/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user != null)
        //    {
        //        _context.Users.Remove(user);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
