using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using BookingBirthday.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
                    PackageLocations = product.PackageLocations
                };
                SaveBookingSession(bookingItem);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(BookingModel request)
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
                        return RedirectToAction("", "Booking");
                    }

                    if (request.Date_start > DateTime.Now)
                    {
                        donHang.Date_start = request.Date_start;
                    }
                    else
                    {
                        TempData["Message"] = "Thời gian tiệc diễn ra sai";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Booking");
                    }

                    if (request.Date_start <= DateTime.Now.AddYears(1))
                    {
                        donHang.Date_start = request.Date_start;
                    }
                    else
                    {
                        TempData["Message"] = "Thời gian tiệc diễn ra không thể vượt quá 1 năm từ ngày hiện tại";
                        TempData["Success"] = false;
                        return RedirectToAction("", "Booking");
                    }

                    donHang.Date_start = request.Date_start;
                    donHang.BookingStatus = "Processing";
                    donHang.Address = request.Address;
                    donHang.Phone = request.Phone;
                    donHang.Note = request.Note;
                    donHang.Email = request.Email;
                    donHang.Total = request.Total;

                    await _dbContext.AddAsync(donHang);
                    await _dbContext.SaveChangesAsync();

                    if (request.CartModels != null)
                    {
                        //foreach (var item in request.CartModels)
                        //{
                        //    var chiTietDonHang = new BookingPackage();
                        //    chiTietDonHang.BookingId = donHang.Id;
                        //    chiTietDonHang.PackageId = item.Package!.Id;
                        //    chiTietDonHang.Price = item.Package!.Price;
                        //    await _dbContext.AddAsync(chiTietDonHang);
                        //    await _dbContext.SaveChangesAsync();
                        //}

                        TempData["Message"] = "Đặt thành công";
                        TempData["Success"] = true;
                        return RedirectToAction("Payment", "Booking", new { bookingId = donHang.Id, userId = donHang.UserId });
                    }
                    else
                    {
                        TempData["Message"] = "Đặt không thành công";
                        TempData["Success"] = false;
                    }
                    return RedirectToAction("", "Booking");
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
                OrderDescription = response.OrderDescription,
                Amount = response.Amount,
                BookingId = int.Parse(response.BookingId)
            };

            _dbContext.Payments.Add(payment);
            _dbContext.SaveChanges();

            var booking = _dbContext.Bookings.Find(int.Parse(response.BookingId));

            _dbContext.SaveChanges();

            TempData["Message"] = $"Thanh toán VNPay thành công";
            return RedirectToAction("Succ", "Booking");
        }

        public IActionResult ViewBookings(int? page)
        {

            //Ràng điều kiện ở đây

            var orders = _dbContext.Bookings.OrderByDescending(x => x.Date_order).Where(x => x.UserId == int.Parse(HttpContext.Session.GetString("user_id")!)).ToList();
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
