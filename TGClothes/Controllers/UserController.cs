using BotDetect.Web.Mvc;
using Data.EF;
using Data.Services;
using Facebook;
using GoogleAuthentication.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Common;
using TGClothes.Models;

namespace TGClothes.Controllers
{
    public class UserController : Controller
    {
        private readonly IAccountService _userService;

        public UserController(IAccountService userService)
        {
            _userService = userService;
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        // GET: User
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "RegisterCaptcha", "Captcha không đúng, vui lòng thử lại!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (_userService.CheckEmailExist(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new Account();
                    user.Email = model.Email;
                    user.Name = model.Name;
                    user.Phone = model.Phone;
                    user.Address = model.Address;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.CreatedDate = DateTime.Now;
                    user.Status = true;

                    var result = _userService.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký tài khoản thành công";
                        model = new RegisterModel();
                        return View(model);
                    }
                    else
                    {
                        ViewBag.Success = "Đăng ký tài khoản không thành công";
                    }
                }
            }
            return View(model);
        }

        public ActionResult Login()
        {
            var clientId = ConfigurationManager.AppSettings["GoogleId"];
            var url = "https://localhost:44362/signin-google";
            var response = GoogleAuth.GetAuthUrl(clientId, url);
            ViewBag.Response = response;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.LoginByEmail(model.Email, Encryptor.MD5Hash(model.Password), false);
                if (result == 1)
                {
                    var user = _userService.GetUserByEmail(model.Email);
                    var userSession = new UserLogin();
                    userSession.Email = user.Email;
                    userSession.Name = user.Name;
                    userSession.UserId = user.Id;
                    Session.Add(CommonConstants.USER_SESSION, userSession);

                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa, vui lòng liên hệ quản trị hoặc đăng nhập bằng tài khoản khác.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không chính xác.");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Mật khẩu không chính xác.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập thất bại.");
                }
            }
            return View(model);
        }

        public ActionResult LoginWithFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbSecretKey"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbSecretKey"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstName = me.first_name;
                string middleName = me.middle_name;
                string lastName = me.last_name;

                var user = new Account();
                user.Email = email;
                user.Status = true;
                user.Name = firstName+ " " + middleName + " " + lastName;
                user.CreatedDate = DateTime.Now;

                var data = _userService.InsertForFacebook(user);
                if (data > 0)
                {
                    var userSession = new UserLogin();
                    userSession.Email = user.Email;
                    userSession.Name = user.Name;
                    userSession.UserId = data;
                    Session.Add(CommonConstants.USER_SESSION, userSession);

                    return Redirect("/");
                }
                else if (data == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa, vui lòng liên hệ quản trị hoặc đăng nhập bằng tài khoản khác.");
                }
            }
                return RedirectToAction("Login");
        }

        public async Task<ActionResult> LoginWithGoogle(string code)
        {
            var clientId = ConfigurationManager.AppSettings["GoogleId"];
            var clientSecret = ConfigurationManager.AppSettings["GoogleSecretKey"];
            var url = "https://localhost:44362/signin-google";
            var token = await GoogleAuth.GetAuthAccessToken(code, clientId, clientSecret, url);
            var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());

            var userProfile2 = await GetGoogleUserProfile(token.AccessToken);

            // Sử dụng thông tin người dùng ở đây hoặc lưu vào cơ sở dữ liệu
            var userName = userProfile2.Name;
            var userEmail = userProfile2.Email;
            var user = new Account();
            user.Email = userEmail;
            user.Name = userName;
            user.Status = true;
            user.CreatedDate = DateTime.Now;

            var data = _userService.InsertForGoogle(user);
            if (data > 0)
            {
                var userSession = new UserLogin();
                userSession.Email = user.Email;
                userSession.Name = user.Name;
                userSession.UserId = data;
                Session.Add(CommonConstants.USER_SESSION, userSession);
            }
            return Redirect("/");
        }

        private async Task<GoogleUserInfo> GetGoogleUserProfile(string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync("https://www.googleapis.com/oauth2/v1/userinfo");
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

        [HttpPost]
        public void SendVerificationLinkEmail(string email, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            //var fromEmail = new MailAddress(ConfigurationManager.AppSettings["FromEmailAddress"], "TGClothes");
            //var toEmail = new MailAddress(email);
            //var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"];

            //string subject = "";
            //string body = "";

            string subject = "Đặt lại mật khẩu";
            string body = "Bạn vừa gửi link đặt lại mật khẩu, Hãy click vào link bên dưới để đặt lại<br>" +
                "<a href=" + link + ">Đặt lại mật khẩu</a>";

            new MailHelper().SendMail(email, "Đặt lại mật khẩu", body);
            //MailMessage mc = new MailMessage(ConfigurationManager.AppSettings["FromEmailAddress"].ToString(), email);
            //mc.Subject = subject;
            //mc.Body = body;
            //mc.IsBodyHtml = true;
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //smtp.Timeout = 1000000;
            //smtp.EnableSsl = true;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //NetworkCredential nc = new NetworkCredential(ConfigurationManager.AppSettings["FromEmailAddress"].ToString(), ConfigurationManager.AppSettings["FromEmailPassword"].ToString());
            //smtp.UseDefaultCredentials = false;
            //smtp.Credentials = nc;
            //smtp.Send(mc);
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
                return false; // suggested by @TK-421
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
                var user = _userService.GetUserByEmail(email);

                if (user != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(user.Email, resetCode, "ResetPassword");
                    user.ResetPasswordCode = resetCode;
                    _userService.Update(user);

                    message = "Link đặt lại mật khẩu đã được gửi đến email của bạn";
                }


                ViewBag.Message = message;
                return View();
            }
            else
            {
                TempData["mgss"] = "Email không hợp lệ";
                return View();
            }

        }

        public ActionResult ResetPassword(string id)
        {
            var user = _userService.GetUserByResetPasswordCode(id);
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByResetPasswordCode(model.ResetCode);
                if (user != null)
                {
                    user.Password = Encryptor.MD5Hash(model.NewPassword);
                    user.ResetPasswordCode = "";

                    _userService.Update(user);
                    message = "Cập nhập mật khẩu thành công";
                    ViewBag.Message = message;
                    return RedirectToAction("Login", "User");
                }
            }
            else
            {
                message = "Cập nhập mật khẩu thất bại";
            }
            ViewBag.Message = message;
            return View(model);
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }

        //public ActionResult AddUserInfo(CustomerInfo model) { }

        //public ActionResult GetProvinces()
        //{
        //    string json = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/js/provinces-api.json"));
        //    Root root = JsonConvert.DeserializeObject<Root>(json);

        //    return View(root);
        //}

        private ActionResult GetDistricts()
        {
            string json = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/js/provinces-api.json"));
            var provinces = JsonConvert.DeserializeObject(json);
                        
            return Json(provinces, JsonRequestBehavior.AllowGet);
        }
    }
}