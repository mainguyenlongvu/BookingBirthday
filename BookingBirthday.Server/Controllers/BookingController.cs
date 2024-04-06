using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using BookingBirthday.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using X.PagedList;

namespace BookingBirthday.Server.Controllers
{
    public class BookingController : BaseController
    {
        private readonly BookingDbContext _dbContext;
        private readonly IVnPayService _vnPayService;

        public BookingController(BookingDbContext dbContext, IVnPayService vnPayService)
        {
            _dbContext = dbContext;
            _vnPayService = vnPayService;
        }

        public const string BOOKINGKEY = "booking";

        public PackageModel GetBookingItem()
        {
            var session = HttpContext.Session;
            string jsonbooking = session.GetString(BOOKINGKEY)!;
            if (jsonbooking != null)
            {
                return JsonConvert.DeserializeObject<PackageModel>(jsonbooking);
            }
            return null;
        }

        void ClearBookingSession()
        {
            var session = HttpContext.Session;
            session.Remove(BOOKINGKEY);
        }

        void SaveBookingSession(Models.PackageModel package)
        {
            var session = HttpContext.Session;

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string jsonbooking = JsonConvert.SerializeObject(package, settings);
            session.SetString(BOOKINGKEY, jsonbooking);
        }

        [Route("/Booking", Name = "Booking")]
        public IActionResult Index()
        {
            return View(GetBookingItem());
        }

        public IActionResult Succ()
        {
            return View();
        }

        [Route("AddToBooking/{productid:int}", Name = "AddToBooking")]
        public IActionResult AddToBooking([FromRoute] int productid)
        {
            var product = _dbContext.Packages.Include(p => p.PackageLocations).ThenInclude(pl => pl.Location)
        .FirstOrDefault(p => p.Id == productid);

            if (product == null)
            {
                return NotFound("Không có gói tiệc này");
            }
            else
            {
                ClearBookingSession();
                var bookingItem = new Models.PackageModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Host_name = product.Host_name,
                    Price = product.Price,
                    Detail = product.Detail,
                    Note = product.Note,
                    Status = product.Status,
                    Gender = product.Gender,
                    Age = product.Age,
                    PackageType = product.PackageType,
                    
                    Theme = product.Theme,
                    PackageLocations = product.PackageLocations
                };
                SaveBookingSession(bookingItem);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(int packageId, BookingModel request)
        {
            if (HttpContext.Session.GetString("user_id") != null)
            {
                var userId = int.Parse(HttpContext.Session.GetString("user_id")!);

                try
                {
                    request.UserId = userId;
                    var booking = new Booking();
                    booking.UserId = request.UserId;
                    booking.Date_order = DateTime.Now;
                    int childAge = DateTime.Today.Year - request.ChildDateOfBirth.Year;
                    var product = _dbContext.Packages.FirstOrDefault(p => p.Id == packageId);

                    if (!Regex.IsMatch(request.Phone, @"^(0[0-9]{9,10})$"))
                    {
                        TempData["Message"] = "Số điện thoại không hợp lệ";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Booking");
                    }

                    if (!request.Email.EndsWith("@gmail.com"))
                    {
                        TempData["Message"] = "Email phải là địa chỉ theo định dạng XXX@gmail.com";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Booking");
                    }

                    if (request.ChildName.Length > 30)
                    {
                        TempData["Message"] = " Tên bé không được vượt quá 30 kí tự";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Booking");
                    }

                    if (request.ChildDateOfBirth > DateTime.Now)
                    {
                        TempData["Message"] = " Ngày tháng năm sinh của bé không hợp lệ";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Booking");
                    }

                    //if (childAge >= 18)
                    //{
                    //    TempData["Message"] = " Ngày tháng năm sinh của bé không được lớn hơn 17 tuổi";
                    //    TempData["Success"] = false;
                    //    return RedirectToAction("", "Booking");
                    //}
                    //else if ((product.Age == "0 - 5" && childAge > 5) || (product.Age == "6 - 10" && (childAge < 6 || childAge > 11)) ||
                    //    (product.Age == "11 - 14" && (childAge < 11 || childAge > 14)) || (product.Age == "15 - 17" && (childAge < 15 || childAge > 17)))
                    //{
                    //    TempData["Message"] = " Ngày tháng năm sinh của bé không phù hợp với gói này";
                    //    TempData["Success"] = false;
                    //    return RedirectToAction("", "Booking");
                    //}

                    //if ((product.Gender == "Nam" && request.ChildGender != "Nam") || (product.Gender == "Nữ" && request.ChildGender != "Nữ"))
                    //{
                    //    TempData["Message"] = " Giới tính của bé không phù hợp với gói này";
                    //    TempData["Success"] = false;
                    //    return RedirectToAction("", "Booking");
                    //}

                    if (request.ChildNumber == 0)
                    {
                        TempData["Message"] = " Số lượng bé tham dự không được bằng 0";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Booking");
                    }

                    if (request.Date_start < DateTime.Now)
                    {
                        TempData["Message"] = "Thời gian tổ chức không được ở quá khứ";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Booking");
                    }

                    if (request.Date_start > DateTime.Now.AddYears(1))
                    {
                        TempData["Message"] = "Thời gian tổ chức không thể vượt quá 1 năm từ ngày hiện tại";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Booking");
                    }

                    if (request.Date_start.TimeOfDay < new TimeSpan(9, 0, 0) || request.Date_start.TimeOfDay > new TimeSpan(19, 0, 0))
                    {
                        TempData["Message"] = "Thời gian tổ chức phải từ 9:00 AM đến 7:00 PM";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Booking");
                    }

                    booking.Date_start = request.Date_start;
                    booking.Phone = request.Phone;
                    booking.Note = request.Note;
                    booking.Email = request.Email;
                    booking.ChildName = request.ChildName;
                    booking.ChildDateOfBirth = request.ChildDateOfBirth;
                    booking.ChildGender = request.ChildGender;
                    booking.ChildNumber = request.ChildNumber;
                    booking.LocationId = int.Parse(request.LocationId);
                    booking.Total = request.Total;
                    booking.PackageId = packageId;
                    booking.CheckIn = null;
                    booking.CheckOut = null;
                    await _dbContext.AddAsync(booking);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Payment", "Booking", new { bookingId = booking.Id, userId = booking.UserId });
                }
                catch (Exception)
                {
                    TempData["Message"] = "Lỗi";
                    TempData["Success"] = false;
                    return RedirectToAction("", "Booking");
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

        public IActionResult CreatePaymentUrl(PaymentInformationModel model, int bookingId)
        {
            ViewData["BookingId"] = bookingId;
            var url = _vnPayService.CreatePaymentUrl(bookingId, model, HttpContext);
            return Redirect(url);
        }

        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            var payment = new Payment
            {
                Date = DateTime.Now,
                Success = response.Success,
                Token = response.Token,
                VnPayResponseCode = response.VnPayResponseCode,
                Amount = response.Amount,
                BookingId = int.Parse(response.BookingId)
            };

            _dbContext.Payments.Add(payment);
            _dbContext.SaveChanges();

            var booking = _dbContext.Bookings.Find(int.Parse(response.BookingId));
            if (booking.BookingStatus == null)
            {
                booking.BookingStatus = "Processing";
                _dbContext.SaveChanges();
                TempData["Message"] = $"Thanh toán VNPay thành công";
                return RedirectToAction("Succ", "Booking");
            }
            else
            {
                booking.BookingStatus = "Paid";
                _dbContext.SaveChanges();
                TempData["Message"] = $"Thanh toán VNPay thành công";
                return RedirectToAction("ViewBookings", "Booking");
            }
        }

        public IActionResult ViewBookings(int? page)
        {
            var orders = _dbContext.Bookings.OrderByDescending(x => x.Date_order).Where(x => x.UserId == int.Parse(HttpContext.Session.GetString("user_id")!) && x.BookingStatus != null).ToList();
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Booking> lst = new PagedList<Booking>(orders, pageNumber, pageSize);
            return View(lst);
        }
        //public IActionResult ViewBooking(int Id)
        //{
        //    var session = HttpContext.Session;
        //    List<Category_requests> category_request = null!;
        //    var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);

        //    if (category_request != null)
        //        {
        //            var jsonNotification = JsonConvert.SerializeObject(category_request);
        //            session.SetString("notification", jsonNotification);
        //        }
        //    //}


        //    var query = from a in _dbContext.BookingPackages
        //                from b in _dbContext.Bookings
        //                on a.BookingId equals b.Id
        //                where b.UserId == user_id && a.BookingId == Id
        //                select new { a };
        //    if (query != null)
        //    {
        //        var data = query.Select(x => new BookingPackageModel()
        //        {
        //            Booking_Package_Id = x.a.Id,
        //            Booking_id = Id,
        //            Package_Id = x.a.PackageId,
        //            Package_name = x.a.Package!.Name,
        //            price = x.a.Price,
        //        }).ToList();
        //        return Json (data);
        //    }
        //    return Json(null);
        //}
        [HttpPost]
        public IActionResult HuyDon(int orderId)
        {
            try
            {
                var data = _dbContext.Bookings.Find(orderId);
                if (data != null)
                {
                    data.BookingStatus = "Declined";
                    data.Date_cancel = DateTime.Now;
                    _dbContext.SaveChanges();
                    TempData["Message"] = "Nhân viên sẽ liên hệ hoàn cọc nếu bạn hủy đúng thời gian quy định";
                    TempData["Success"] = true;
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
    }
}
