using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Common;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace BookingBirthday.Server.Controllers
{
    public class PackageController : Controller
    {
        private readonly BookingDbContext _dbContext;

        public PackageController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Show(int id)
        {
            int userId;
            HttpContext.Session.SetString("packageid", id.ToString());
            var userIdSession = HttpContext.Session.GetString("user_id");
            if (userIdSession == null)
            {
                userId = 0;
            }
            else
            {
                userId = int.Parse(userIdSession);
            }
            var package = _dbContext.Packages.Find(id);
            ViewBag.Review = from r in _dbContext.Rates.ToList()
                             join u in _dbContext.Users.ToList() on r.UserId equals u.Id
                             where (r.PackageId == id && r.UserId == u.Id)
                             select new UserRateModel
                             {
                                 Rate = r,
                                 User = u
                             };

            var ratings = _dbContext.Rates.Where(r => r.PackageId == id && r.UserId == userId).ToList();

            if (ratings.Any())
            {
                ViewBag.PackageRated = true;
            }
            else
            {
                ViewBag.PackageRated = false;
            }

            if (userId >= 0)
            {
                var data = from booking in _dbContext.Bookings
                           where booking.PackageId == id && booking.BookingStatus == "Paid"
                           select booking;
                ViewBag.CustomerPurchased = data.Count();
            }

            HttpContext.Session.SetString("package_id", package.Id.ToString());
            ViewBag.CountRate = CountRate(id);
            ViewBag.CountRateFiveStar = CountRateFiveStar(id);
            if (ViewBag.CountRate == 0)
            {
                ViewBag.AverageRate = null;
            }
            else
            {
                var rate = _dbContext.Rates.Where(x => x.Star > 0 && x.PackageId == id);
                var rateSum = rate != null ? rate.Sum(x => x.Star) : 0;
                var countRate = rate.Count();
                var avgRate = (float)rateSum / countRate;

                ViewBag.AverageRate = avgRate;
            }

            var session = HttpContext.Session;
            var role = HttpContext.Session.GetString("role");
            List<Category_requests> category_request = null!;

            if (role == "Admin")
            {
                category_request = _dbContext.Category_Requests
                    .Where(x => x.is_approved == 0 && x.is_deleted_by_admin == false)
                    .OrderByDescending(x => x.created_at)
                    .ToList();
            }
            else if (role == "Host")
            {
                var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
                category_request = _dbContext.Category_Requests
                    .Where(x => x.is_deleted_by_owner == false && x.requester_id == user_id && (x.is_approved == -1 || x.is_approved == 1))
    .OrderByDescending(x => x.created_at)
                    .ToList();
            }

            if (category_request != null)
            {
                var jsonNotification = JsonConvert.SerializeObject(category_request);
                session.SetString("notification", jsonNotification);
            }

            package = _dbContext.Packages.Where(x => x.Id == id).FirstOrDefault();
            if (package != null)
            {
                var p = new PackageModel();
                p.Id = package.Id;
                p.Name = package.Name;
                p.Detail = package.Detail;
                p.Note = package.Note;
                p.Price = package.Price;
                p.image_url = package.image_url;
                p.Status = package.Status;
                p.Host_name = package.Host_name;
                p.UserId = package.UserId;
                return View(p);
            }

            ViewBag.PackageId = id;

            return View(package);
        }

        public int CountRate(int id)
        {
            List<Package> package = _dbContext.Packages.ToList();
            List<Rate> rate = _dbContext.Rates.ToList();
            int countRate = (from r in rate
                             join p in package
                             on r.PackageId equals p.Id
                             where (r.PackageId == id && r.PackageId == p.Id && r.Star > 0)
                             select new PackageRateModel
                             {
                                 Rate = r,
                                 Package = p
                             }).Count();
            return countRate;
        }

        public int CountRateFiveStar(int id)
        {
            List<Package> package = _dbContext.Packages.ToList();
            List<Rate> rate = _dbContext.Rates.ToList();
            int countRate = (from r in rate
                             join p in package
                             on r.PackageId equals p.Id
                             where (r.PackageId == id && r.PackageId == p.Id && r.Star == 5)
                             select new PackageRateModel
                             {
                                 Rate = r,
                                 Package = p
                             }).Count();
            return countRate;
        }

        [HttpPost]
        public ActionResult PackageReviews(Rate review, int rating, string content)
        {
            var userId = int.Parse(HttpContext.Session.GetString("user_id"));
            var packageId = int.Parse(HttpContext.Session.GetString("package_id"));
            List<int> stars = _dbContext.Rates.Where(x => x.UserId == userId && x.PackageId == packageId).Select(x => x.Star).ToList();
            if (stars.Count() == 0)
            {
                if (rating > 0)
                {
                    review.CreatedDate = DateTime.Now;
                    review.Content = content;
                    review.UserId = userId;
                    review.Star = rating;
                    review.PackageId = packageId;
                    Insert(review);
                }
                else
                {
                    TempData["Message"] = "Vui nhập chọn số sao muốn đánh giá.";
                    TempData["Success"] = false;
                    return Redirect($"/Package/Show?Id={packageId}");
                }
            }
            else
            {
                TempData["Message"] = "Bạn đã đánh giá bữa tiệc này rồi.";
                TempData["Success"] = false;
                return Redirect($"/Package/Show?Id={packageId}");
            }

            return Redirect($"/Package/Show?Id={packageId}");
        }

        public Rate Insert(Rate rate)
        {
            _dbContext.Rates.Add(rate);
            _dbContext.SaveChanges();
            return rate;
        }

        public IActionResult ProfileHost(int UserId, int page = 1) // Giá trị mặc định cho page là 1
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == UserId);
            if (user == null)
            {
                // Xử lý trường hợp không tìm thấy user
            }

            // Truy vấn danh sách package
            var packagesQuery = _dbContext.Packages.Where(p => p.UserId == UserId);

            int pageSize = 4; // Hoặc bất kỳ giá trị nào bạn muốn
            var pagedPackages = packagesQuery.OrderBy(p => p.Id) // Hoặc bất kỳ tiêu chí sắp xếp nào bạn muốn
                .ToPagedList(page, pageSize);

            var viewModel = new ProfileViewModel
            {
                User = user,
                PagedPackages = pagedPackages
            };

            return View(viewModel);
        }
    }
}