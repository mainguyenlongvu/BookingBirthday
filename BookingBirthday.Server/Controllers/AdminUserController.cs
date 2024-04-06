using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Common;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using X.PagedList;

namespace BookingBirthday.Server.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly BookingDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imageContentFolder;
        private readonly IConfiguration _configuration;

        public AdminUserController(BookingDbContext dbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _imageContentFolder = Path.Combine(webHostEnvironment.WebRootPath, "imgProfile");
            _configuration = configuration;
        }
        public IActionResult Index(int? page)
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

            var users = _dbContext.Users.OrderByDescending(x => x.Id).Where(x=> x.Role != "Admin").ToList();
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<User> lst = new PagedList<User>(users, pageNumber, pageSize);
            return View(lst);
        }
        [HttpPost]
        public IActionResult Create(RegisterModel userData)
        {
            try
            {
                if (userData.Name.Length > 50)
                {
                    TempData["Message"] = " Tên người dùng không được vượt quá 50 kí tự";
                    TempData["Success"] = false;
                    return View(userData);
                }

                if (userData.Username.Length < 8 || userData.Username.Length > 30)
                {
                    TempData["Message"] = " Tài khoản phải chứa từ 8 đến 30 kí tự";
                    TempData["Success"] = false;
                    return View(userData);
                }

                if (userData.Password != userData.ConfirmPassword)
                {
                    TempData["Message"] = " Mật khẩu xác nhận không đúng";
                    TempData["Success"] = false;
                    return View(userData);
                }

                if (userData.Password.Length < 8 || userData.Password.Length > 30)
                {
                    TempData["Message"] = " Mật khẩu phải chứa từ 8 đến 30 kí tự";
                    TempData["Success"] = false;
                    return View(userData);
                }

                if (!Regex.IsMatch(userData.Phone, @"^(0[0-9]{9,10})$"))
                {
                    TempData["Message"] = " Số điện thoại không hợp lệ";
                    TempData["Success"] = false;
                    return View(userData);
                }

                if (!userData.Email.EndsWith("@gmail.com"))
                {
                    TempData["Message"] = " Email phải là địa chỉ theo định dạng XXX@gmail.com";
                    TempData["Success"] = false;
                    return View(userData);
                }

                if (userData.DateOfBirth > DateTime.Now || userData.DateOfBirth < DateTime.Now.AddYears(-100))
                {
                    TempData["Message"] = " Ngày tháng năm sinh không hợp lệ";
                    TempData["Success"] = false;
                    return View(userData);
                }

                if (userData.Address.Length > 200)
                {
                    TempData["Message"] = " Địa chỉ không được vượt quá 200 kí tự";
                    TempData["Success"] = false;
                    return View(userData);
                }
                var user = new Data.Entities.User();
                user.Username = userData.Username!;
                user.Role= userData.Role;
                user.Status = "Active";
                user.Password = CreateMD5.MD5Hash(userData.Password!);
                user.Email = userData.Email!;
                user.Address = userData.Address;
                user.Phone = userData.Phone;
                user.Name = userData.Name;
                user.DateOfBirth = userData.DateOfBirth;
                user.Gender = userData.Gender;
                if (userData.file != null)
                {
                    user.Image_url = UploadedFile(userData.file!);
                }
                else
                {
                    user.Image_url = "/imgProfile/avatar.png";
                }
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
                var user = _dbContext.Users.SingleOrDefault(x => x.Id == userData.User_id);
                if(userData.Role == "Guest")
                {
                    var packagesToUpdate = (from a in _dbContext.Packages
                                            join b in _dbContext.Users on a.UserId equals b.Id
                                            where a.UserId == b.Id && b.Status == "Active"
                                            select a).ToList();

                    foreach (var package in packagesToUpdate)
                    {
                        package.Status = "InActive";
                    }
                    _dbContext.SaveChanges();
                }
                user.Role = userData.Role;
                user.Status = userData.Status;

                if (userData.Status == "InActive")
                {
                    var packagesToUpdate = (from a in _dbContext.Packages
                                            join b in _dbContext.Users on a.UserId equals b.Id
                                            where a.UserId == b.Id && b.Status == "Active"
                                            select a).ToList();

                    foreach (var package in packagesToUpdate)
                    {
                        package.Status = "InActive";
                    }
                    _dbContext.SaveChanges();
                }
                else if(userData.Status == "Active")
                
                {
                    var packagesToUpdate = (from a in _dbContext.Packages
                                            join b in _dbContext.Users on a.UserId equals b.Id
                                            where a.UserId == b.Id && b.Status == "InActive"
                                            select a).ToList();

                    foreach (var package in packagesToUpdate)
                    {
                        package.Status = "Active";
                    }
                    _dbContext.SaveChanges();
                }
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


        private string UploadedFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null; // Tránh truy cập vào thuộc tính FileName của null

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(_imageContentFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return "/imgProfile/" + uniqueFileName;
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
