using Microsoft.AspNetCore.Mvc;
using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using BookingBirthday.Server.Common;
using System.Text.RegularExpressions;
using GoogleAuthentication.Services;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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

        // POST: Users/Login
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
            var userProfile2 = JsonConvert.DeserializeObject<UserLogin>(userProfile);
            //var userProfile2 = await GetGoogleUserProfile(userProfile);

            // Sử dụng thông tin người dùng ở đây hoặc lưu vào cơ sở dữ liệu
            var userName = userProfile2.Name;
            var userEmail = userProfile2.Email;
            var userPicture = userProfile2.Picture;
            var user = new User();
            user.Email = userEmail;
            user.Name = userName;
            user.Password = "123";
            user.Status = "Active";
            user.Role = "Guest";
            user.Username = userEmail;
            user.Phone = "";
            user.Address = "";
            user.Image_url = userPicture;

            var data = InsertForGoogle(user);
            if (data > 0)
            {
                var userSession = new UserLogin();
                userSession.Email = user.Email;
                userSession.Name = user.Name;
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("role", user.Role!);
                HttpContext.Session.SetString("status", user.Status!);
                HttpContext.Session.SetString("user_id", user.Id.ToString()!);
                HttpContext.Session.SetString("name", user.Name!);
                HttpContext.Session.SetString("email", user.Email!);
                HttpContext.Session.SetString("phone", user.Phone!);
                HttpContext.Session.SetString("address", user.Address!);
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

        private async Task<GoogleUserInfo> GetGoogleUserProfile(string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var userProfile = JsonConvert.DeserializeObject<GoogleUserInfo>(content);
                    return userProfile;
                }
                else
                {
                    // Xử lý lỗi khi gọi API
                    throw new Exception("Failed to get user profile from Google API.");
                }
            }
        }
        public long InsertForGoogle(User user)
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

        // POST: Users/Register
        public IActionResult Register()
        {
            var clientId = _configuration["Authentication:Google:ClientId"];
            var url = "https://localhost:7297/Account/RegisterWithGoogle";
            var response = GoogleAuth.GetAuthUrl(clientId, url);
            ViewBag.Response = response;
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel userData)
        {
            try
            {
                // Kiểm tra tài khoản
                if (userData.Username.Length < 8)
                {
                    TempData["Message"] = "Tài khoản phải chứa ít nhất 8 ký tự.";
                    TempData["Success"] = false;
                    return View(userData);
                }

                // Kiểm tra mật khẩu và mật khẩu xác nhận
                if (userData.Password != userData.ConfirmPassword)
                {
                    TempData["Message"] = "Mật khẩu xác nhận không đúng";
                    TempData["Success"] = false;
                    return View(userData);
                }

                // Kiểm tra mật khẩu
                if (userData.Password.Length < 8)
                {
                    TempData["Message"] = "Mật khẩu phải chứa ít nhất 8 ký tự.";
                    TempData["Success"] = false;
                    return View(userData);
                }

                // Kiểm tra số điện thoại
                if (!Regex.IsMatch(userData.Phone, @"^(0[0-9]{9,10})$"))
                {
                    TempData["Message"] = "Số điện thoại không hợp lệ.";
                    TempData["Success"] = false;
                    return View(userData);
                }

                // Kiểm tra email
                if (!userData.Email.EndsWith("@gmail.com"))
                {
                    TempData["Message"] = "Email phải là địa chỉ theo format XXX@gmail.com";
                    TempData["Success"] = false;
                    return View(userData);
                }

                var usr = _dbContext.Users.Where(x => x.Username == userData.Username || x.Email == userData.Email);
                if (usr.Count() > 0)
                {
                    TempData["Message"] = "Tài khoản đã tồn tại";
                    TempData["Success"] = false;
                    return View(userData);
                }

                var user = new User();
                user.Username = userData.Username!;
                user.Role = "Guest";
                user.Status = "Active";
                user.Password = CreateMD5.MD5Hash(userData.Password!);
                user.Email = userData.Email!;
                user.Address = userData.Address;
                user.Phone = userData.Phone;
                user.Name = userData.Name;
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
                            TempData["Message"] = "Tài khoản đã tồn tại";
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
                    user.Email = userData.Email;
                    user.Address = userData.Address;
                    user.Phone = userData.Phone;
                    user.Name = userData.Name;
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
