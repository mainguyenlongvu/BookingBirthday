using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using BookingBirthday.Data.EF;
using BookingBirthday.Server.Services;


namespace BookingBirthday.Server.Controllers
{
    public class CartController : GuestBaseController
    {
        private readonly BookingDbContext _dbContext;
        
        public CartController(BookingDbContext appContext)
        {
            _dbContext = appContext;
            
        }

        public const string CARTKEY = "cart";
        public List<CartModel> GetCartItems()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY)!;
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartModel>>(jsoncart)!;
            }
            return new List<CartModel>();
        }

        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        void SaveCartSession(List<Models.CartModel> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

        [Route("/cart", Name = "cart")]
        public IActionResult Index()
        {
            //var session = HttpContext.Session;
            //var role = HttpContext.Session.GetString("role");
            //List<Category_requests> category_request = null!;

            //if (role != null)
            //{

            //    if (role == "Admin")
            //    {
            //        category_request = _dbContext.Category_Requests
            //            .Where(x => x.is_approved == 0 && x.is_deleted_by_admin == false)
            //            .OrderByDescending(x => x.created_at)
            //            .ToList();
            //    }
            //    else if (role == "Host")
            //    {

            //        var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
            //        category_request = _dbContext.Category_Requests
            //            .Where(x => x.is_deleted_by_owner == false && x.requester_id == user_id && (x.is_approved == -1 || x.is_approved == 1))
            //            .OrderByDescending(x => x.created_at)
            //            .ToList();
            //    }

            //    if (category_request != null)
            //    {
            //        var jsonNotification = JsonConvert.SerializeObject(category_request);
            //        session.SetString("notification", jsonNotification);
            //    }
            //}

            return View(GetCartItems());
        }

        public IActionResult Succ()
        {
            return View();
        }

        [Route("addcart/{productid:int}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute] int productid)
        {

            var product = _dbContext.Packages
                .Where(p => p.Id == productid )
                .FirstOrDefault();
            if (product == null)
                return NotFound("Không có sản phẩm");


            var cart = GetCartItems();

            //var cartitem = cart.Find(p => p.Package!.Id == productid);

            if (cart.Count >=1)
            {
                TempData["Message"] = "Giỏ hàng đã có bữa tiệc";
                TempData["Success"] = false;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                cart.Add(new Models.CartModel() {  Package = product });
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Index));
        }
        
        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int productid)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.Package!.Id == productid);
            if (cartitem != null)
            {
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(BookingModel request)
        {
            if (HttpContext.Session.GetString("user_id") != null)
            {
                var userId = int.Parse(HttpContext.Session.GetString("user_id")!);

                try
                {
                    request.UserId = userId;
                    var donHang = new Booking();
                    donHang.UserId = request.UserId;
                    donHang.Date_order = DateTime.Now;

                    if (request.Date_start != null)
                    {
                        donHang.Date_start = request.Date_start;
                    }
                    else
                    {
                        TempData["Message"] = "Không được để trống thời gian tiệc diễn ra";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Cart");
                    }

                    if (request.Date_start > DateTime.Now)
                    {
                        donHang.Date_start = request.Date_start;
                    }
                    else
                    {
                        TempData["Message"] = "Thời gian tiệc diễn ra sai";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Cart");
                    }

                    if (request.Date_start <= DateTime.Now.AddYears(1))
                    {
                        donHang.Date_start = request.Date_start;
                    }
                    else
                    {
                        TempData["Message"] = "Thời gian tiệc diễn ra không thể vượt quá 1 năm từ ngày hiện tại";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Cart");
                    }

                    donHang.Date_start = request.Date_start;
                    donHang.BookingStatus = "Processing";
                    //donHang.Address = request.Address;
                    donHang.Phone = request.Phone;
                    donHang.Note = request.Note;
                    donHang.Email = request.Email;
                    donHang.Total = request.Total;

                    await _dbContext.AddAsync(donHang);
                    await _dbContext.SaveChangesAsync();

                    //if (request.CartModels != null)
                    //{
                    //    //foreach (var item in request.CartModels)
                    //    //{
                    //    //    var chiTietDonHang = new BookingPackage();
                    //    //    chiTietDonHang.BookingId = donHang.Id;
                    //    //    chiTietDonHang.PackageId = item.Package!.Id;
                    //    //    chiTietDonHang.Price = item.Package!.Price;
                    //    //    await _dbContext.AddAsync(chiTietDonHang);
                    //    //    await _dbContext.SaveChangesAsync();
                    //    //}

                    //    TempData["Message"] = "Đặt thành công";
                    //    TempData["Success"] = true;
                    //    return RedirectToAction("Payment", "Cart", new { bookingId = donHang.Id, userId = donHang.UserId });
                    //}
                    //else
                    //{
                    //    TempData["Message"] = "Đặt không thành công";
                    //    TempData["Success"] = false;
                    //}
                    //return RedirectToAction("", "Cart");
                }
                catch (Exception)
                {
                    TempData["Message"] = "Lỗi";
                    TempData["Success"] = false;
                    return RedirectToAction("", "Cart");
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Payment(int bookingId, int userId)
        {
            var booking = _dbContext.Bookings.Find(bookingId);
            var user = _dbContext.Users.Find(userId);

            ViewData["BookingId"] = bookingId;
            ViewData["Total"] = (booking.Total / 2);
            ViewData["Name"] = user.Name;
            return View();
        }

        //public IActionResult DepositPayment(int bookingId, int userId)
        //{
        //    var booking = _dbContext.Bookings.Find(bookingId);
        //    var user = _dbContext.Users.Find(userId);

        //    ViewData["BookingId"] = bookingId;
        //    ViewData["Total"] = (booking.Total / 2);
        //    ViewData["Name"] = user.Name;
        //    return View();
        //}

        //public IActionResult RemainingPayment(int bookingId, int userId)
        //{
        //    var booking = _dbContext.Bookings.Find(bookingId);
        //    var user = _dbContext.Users.Find(userId);

        //    ViewData["BookingId"] = bookingId;
        //    ViewData["Total"] = (booking.Total / 2);
        //    ViewData["Name"] = user.Name;
        //    return View();
        //}

        //public IActionResult CreatePaymentUrl(PaymentInformationModel model, int bookingId)
        //{
        //    ViewData["BookingId"] = bookingId;
        //    var url = _vnPayService.CreatePaymentUrl(bookingId, model, HttpContext);
        //    return Redirect(url);
        //}

        //public IActionResult CreateDepositPaymentUrl(PaymentInformationModel model, int bookingId)
        //{
        //    ViewData["BookingId"] = bookingId;
        //    var url = _vnPayService.CreateDepositPaymentUrl(bookingId, model, HttpContext);
        //    return Redirect(url);
        //}

        //public IActionResult CreateRemainingPaymentUrl(PaymentInformationModel model, int bookingId)
        //{
        //    ViewData["BookingId"] = bookingId;
        //    var url = _vnPayService.CreateRemainingPaymentUrl(bookingId, model, HttpContext);
        //    return Redirect(url);
        //}

        

        //public IActionResult DepositPaymentCallBack()
        //{
        //    var response = _vnPayService.DepositPaymentExecute(Request.Query);

        //    if (response == null || response.VnPayResponseCode != "00")
        //    {
        //        TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
        //        return RedirectToAction("PaymentFail");
        //    }

        //    var depositPayment = new DepositPayment
        //    {
        //        Date = DateTime.Now,
        //        Success = response.Success,
        //        Token = response.Token,
        //        VnPayResponseCode = response.VnPayResponseCode,
        //        OrderDescription = response.OrderDescription,
        //        Amount = response.Amount,
        //        BookingId = int.Parse(response.BookingId)
        //    };

        //    _dbContext.DepositPayments.Add(depositPayment);
        //    _dbContext.SaveChanges();

        //    var booking = _dbContext.Bookings.Find(int.Parse(response.BookingId));

        //    booking.DepositPaymentId = depositPayment.Id;

        //    _dbContext.SaveChanges();

        //    TempData["Message"] = $"Thanh toán VNPay thành công";
        //    return RedirectToAction("Succ", "Cart");
        //}

        //public IActionResult RemainingPaymentCallBack()
        //{
        //    var response = _vnPayService.RemainingPaymentExecute(Request.Query);

        //    if (response == null || response.VnPayResponseCode != "00")
        //    {
        //        TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
        //        return RedirectToAction("PaymentFail");
        //    }

        //    var remainingPayment = new RemainingPayment
        //    {
        //        Date = DateTime.Now,
        //        Success = response.Success,
        //        Token = response.Token,
        //        VnPayResponseCode = response.VnPayResponseCode,
        //        OrderDescription = response.OrderDescription,
        //        Amount = response.Amount,
        //        BookingId = int.Parse(response.BookingId)
        //    };

        //    _dbContext.RemainingPayments.Add(remainingPayment);
        //    _dbContext.SaveChanges();

        //    var booking = _dbContext.Bookings.Find(int.Parse(response.BookingId));

        //    booking.RemainingPaymentId = remainingPayment.Id;
        //    booking.BookingStatus = "Paid";

        //    _dbContext.SaveChanges();

        //    TempData["Message"] = $"Thanh toán VNPay thành công";
        //    return RedirectToAction("ViewBookings", "Booking");
        //}
    }
}
