using Microsoft.AspNetCore.Mvc;
using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using BookingBirthday.Server.Common;
using System.Text.RegularExpressions;
using GoogleAuthentication.Services;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using BookingBirthday.Data.Enums;
using BookingBirthday.Application.Helper;
using System.Net.Mail;
using Microsoft.AspNetCore.Http.Extensions;
using BookingBirthday.Server.Helper;

namespace BookingBirthday.Server.Controllers
{
    public class AccountController : Controller
    {

        private readonly BookingDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imageContentFolder;
        private readonly IConfiguration _configuration;

        public AccountController(BookingDbContext dbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _imageContentFolder = Path.Combine(webHostEnvironment.WebRootPath, "imgProfile");
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            var clientId = _configuration["Authentication:Google:ClientId"];
            var url = "https://localhost:7297/Account/LoginWithGoogle";
            var response = GoogleAuth.GetAuthUrl(clientId, url);
            ViewBag.Response = response;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginData)
        {
            try
            {
                var pwt = CreateMD5.MD5Hash(loginData.Password!);
                var user = _dbContext.Users.FirstOrDefault(x => x.Username == loginData.Username && x.Password == pwt);
                
                if (user != null)
                {
                    if (user!.Status == "InActive")
                    {
                        TempData["Message"] = "Tài khoản của bạn đã bị khóa, vui lòng liên hệ quản trị viên!";
                        TempData["Success"] = false;
                        return View(loginData);
                    }

                    HttpContext.Session.SetString("username", user.Username!);
                    HttpContext.Session.SetString("role", user.Role!);
                    HttpContext.Session.SetString("status", user.Status!);
                    HttpContext.Session.SetString("user_id", user.Id.ToString()!);
                    HttpContext.Session.SetString("name", user.Name!);
                    HttpContext.Session.SetString("email", user.Email!);
                    HttpContext.Session.SetString("phone", user.Phone!);
                    HttpContext.Session.SetString("address", user.Address!);
                    HttpContext.Session.SetString("date", user.DateOfBirth.ToString()!);
                    HttpContext.Session.SetString("gender", user.Gender!);

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
                TempData["Message"] = "Tài khoản không tồn tại";
                TempData["Success"] = false;
                return View(loginData);
            }
            catch(Exception ex)
            {
                TempData["Message"] = "Đăng nhập không thành công";
                TempData["Success"] = false;
                return View(loginData);
            }
        }

        public async Task<ActionResult> LoginWithGoogle(string code)
        {
            var clientId = _configuration["Authentication:Google:ClientId"];
            var clientSecret = _configuration["Authentication:Google:ClientSecret"];
            var url = "https://localhost:7297/Account/LoginWithGoogle";
            var token = await GoogleAuth.GetAuthAccessToken(code, clientId, clientSecret, url);
            var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());
            var userData = JsonConvert.DeserializeObject<UserLogin>(userProfile);

            var userName = userData.Name;
            var userEmail = userData.Email;
            var userPicture = userData.Picture;

            var user = new User();
            user.Name = userName;
            user.Gender = "Nam";
            user.DateOfBirth = DateTime.Now;
            user.Username = userEmail;
            user.Email = userEmail;
            user.Password = CreateMD5.MD5Hash("12345678");
            user.Phone = "";
            user.Address = "";
            user.Role = "Guest";
            user.Status = "Active";
            user.Image_url = userPicture;

            var data = InsertForGoogle(user);
            if (data > 0)
            {
                if (user!.Status == "InActive")
                {
                    TempData["Message"] = "Tài khoản của bạn đã bị khóa, vui lòng liên hệ quản trị viên!";
                    TempData["Success"] = false;
                    return RedirectToAction("Login", "Account");
                }

                HttpContext.Session.SetString("username", user.Username!);
                HttpContext.Session.SetString("role", user.Role!);
                HttpContext.Session.SetString("status", user.Status!);
                HttpContext.Session.SetString("user_id", data.ToString());
                HttpContext.Session.SetString("name", user.Name!);
                HttpContext.Session.SetString("email", user.Email!);
                HttpContext.Session.SetString("phone", user.Phone!);
                HttpContext.Session.SetString("address", user.Address!);
                HttpContext.Session.SetString("date", user.DateOfBirth.ToString()!);
                HttpContext.Session.SetString("gender", user.Gender!);
            }
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

        public int InsertForGoogle(User user)
        {
            var data = _dbContext.Users.SingleOrDefault(x => x.Email == user.Email);
            if (data == null)
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return user.Id;
            }
            else
            {
                return data.Id;
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel userData)
        {
            try
            {
                if (userData.Username.Length < 8)
                {
                    TempData["Message"] = "Tài khoản phải chứa ít nhất 8 ký tự.";
                    TempData["Success"] = false;
                    return View(userData);
                }

                if (userData.Password != userData.ConfirmPassword)
                {
                    TempData["Message"] = "Mật khẩu xác nhận không đúng";
                    TempData["Success"] = false;
                    return View(userData);
                }

                if (userData.Password.Length < 8)
                {
                    TempData["Message"] = "Mật khẩu phải chứa ít nhất 8 ký tự.";
                    TempData["Success"] = false;
                    return View(userData);
                }

                if (!Regex.IsMatch(userData.Phone, @"^(0[0-9]{9,10})$"))
                {
                    TempData["Message"] = "Số điện thoại không hợp lệ.";
                    TempData["Success"] = false;
                    return View(userData);
                }

                if (!userData.Email.EndsWith("@gmail.com"))
                {
                    TempData["Message"] = "Email phải là địa chỉ theo format XXX@gmail.com";
                    TempData["Success"] = false;
                    return View(userData);
                }

                if (userData.DateOfBirth > DateTime.Now)
                {
                    TempData["Message"] = "Ngày tháng năm sinh không hợp lệ.";
                    TempData["Success"] = false;
                    return View(userData);
                }

                var user = new User();
                user.Name = userData.Name;
                user.Gender = userData.Gender;
                user.DateOfBirth = userData.DateOfBirth;
                user.Username = userData.Username!;
                user.Password = CreateMD5.MD5Hash(userData.Password!);
                user.Email = userData.Email!;
                user.Phone = userData.Phone;
                user.Address = userData.Address;
                user.Role = "Guest";
                user.Status = "Active";
               
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

        [HttpPost]
        public void SendVerificationLinkEmail(string email, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = $"/Account/{emailFor}/{activationCode}";
            var link = new Uri(Request.GetEncodedUrl()).GetLeftPart(UriPartial.Authority) + verifyUrl;

            string subject = "Đặt lại mật khẩu";
            string body = $"Bạn vừa gửi link đặt lại mật khẩu. Hãy click vào link bên dưới để đặt lại: <br>" +
                          $"<a href=\"{link}\">Đặt lại mật khẩu</a>";

            new MailHelper().SendMail(email, "Đặt lại mật khẩu", body);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {

            if (IsValidEmail(email))
            {
                string message = "";
                var user = _dbContext.Users.SingleOrDefault(x => x.Email == email);

                if (user != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(user.Email, resetCode, "ResetPassword");
                    user.ResetPasswordCode = resetCode;
                    Update(user);
                    Console.WriteLine(user);

                    message = "Link đặt lại mật khẩu đã được gửi đến email của bạn";
                }

                TempData["mgss"] = message;
                return View();
            }
            else
            {
                TempData["mgss"] = "Email không hợp lệ";
                return View();
            }

        }

        private bool Update(User user)
        {
            try
            {
                var data = _dbContext.Users.Find(user.Id);
                data.Name = user.Name;
                data.Email = user.Email;
                data.Password = user.Password;
                data.ResetPasswordCode = user.ResetPasswordCode;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpGet("/Account/ResetPassword/{resetPass}")]
        public ActionResult ResetPassword(string resetPass)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.ResetPasswordCode == resetPass);
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = resetPass;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                var user = _dbContext.Users.SingleOrDefault(x => x.ResetPasswordCode == model.ResetCode);
                if (user != null)
                {
                    user.Password = CreateMD5.MD5Hash(model.NewPassword);
                    user.ResetPasswordCode = "";

                    Update(user);
                    message = "Cập nhập mật khẩu thành công";
                    ViewBag.Message = message;
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                message = "Cập nhập mật khẩu thất bại";
            }
            ViewBag.Message = message;
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("user_id") != null)
            {
                var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
                var user = _dbContext.Users.FirstOrDefault(x => x.Id == user_id);
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
            try
            {
                if (HttpContext.Session.GetString("user_id") != null)
                {

                    var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
                    var email = HttpContext.Session.GetString("email");
                    if (email != userData.Email)
                    {
                        var usr = _dbContext.Users.Where(x => x.Email == userData.Email);
                        if (usr.Count() > 0)
                        {
                            TempData["Message"] = "Tài khoản email đã tồn tại";
                            TempData["Success"] = false;
                            return RedirectToAction("Profile", "Account");
                        } else if (!userData.Email.EndsWith("@gmail.com"))
                        {
                            TempData["Message"] = "Email phải là địa chỉ theo format XXX@gmail.com";
                            TempData["Success"] = false;
                            return RedirectToAction("Profile", "Account");
                        }
                    }

                    var user = _dbContext.Users.FirstOrDefault(x => x.Id == user_id);
                    if (user == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    else if (userData.Password != userData.ConfirmPassword)
                    {
                        TempData["Message"] = "Mật khẩu xác nhận không đúng";
                        TempData["Success"] = false;
                        return RedirectToAction("Profile", "Account");
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
                            return RedirectToAction("Profile", "Account");
                        }
                    }

                    var phone = HttpContext.Session.GetString("phone");
                    if (phone != userData.Phone)
                    {
                        var usr = _dbContext.Users.Where(x => x.Phone == userData.Phone);
                        if (usr.Count() > 0)
                        {
                            TempData["Message"] = "Số điện thoại đã tồn tại";
                            TempData["Success"] = false;
                            return RedirectToAction("Profile", "Account");
                        } else if (!Regex.IsMatch(userData.Phone, @"^(0[0-9]{9,10})$"))
                        {
                            TempData["Message"] = "Số điện thoại không hợp lệ";
                            TempData["Success"] = false;
                            return RedirectToAction("Profile", "Account");
                        }
                    }
                    HttpContext.Session.SetString("phone", userData.Phone!);

                    var date = HttpContext.Session.GetString("date");

                    if (DateTime.Parse(date) != userData.DateOfBirth)
                    {
                        if (userData.DateOfBirth > DateTime.Now)
                        {
                            TempData["Message"] = "Ngày tháng năm sinh không hợp lệ.";
                            TempData["Success"] = false;
                            return RedirectToAction("Profile", "Account");
                        }
                    }

                    user.Email = userData.Email;
                    user.Address = userData.Address;
                    user.Phone = userData.Phone;
                    user.Name = userData.Name;
                    user.DateOfBirth = userData.DateOfBirth;
                    user.Gender = userData.Gender;

                    if (userData.file != null)
                    {
                        if (user.Image_url != "/imgProfile/avatar.png" && user.Image_url != null)
                        {
                            var n = user.Image_url!.Remove(0, 12);
                            DeleteImage(n);
                        }
                        user.Image_url = UploadedFile(userData.file!);
                    }

                    _dbContext.SaveChanges();
                    TempData["Message"] = "Cập nhật thông tin thành công";
                    TempData["Success"] = true;
                    return RedirectToAction("Profile", "Account");
                }

                return RedirectToAction("Login", "Account");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Lỗi cập nhật thông tin tài khoản";
                TempData["Success"] = false;
                return RedirectToAction("Profile", "Account");
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
