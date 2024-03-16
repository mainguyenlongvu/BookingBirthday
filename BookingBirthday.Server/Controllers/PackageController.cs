using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookingBirthday.Server.Controllers
{
    public class PackageController : Controller
    {
        private readonly BookingDbContext _dbContext;

        public PackageController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Show(int Id)
        {
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

            //var package = (from a in _dbContext.Packages
            //               join b in _dbContext.Users on a.UserId equals b.Id
            //               where a.Id == Id && b.Status == "Active"
            //               select new PackageModel
            //               {
            //                   Id = a.Id,
            //                   Name = a.Name,
            //                   Venue = a.Venue,
            //                   Detail = a.Detail,
            //                   Note = a.Note,
            //                   Price = a.Price,
            //                   image_url = a.image_url
            //               }).FirstOrDefault();

            //if (package != null)
            //{
            //    return View(package);
            //}
            //return null;

            var product = _dbContext.Packages.Where(x => x.Id == Id).FirstOrDefault();
            if (product != null)
            {
                var p = new PackageModel();
                p.Id = product.Id;
                p.Name = product.Name;
                p.Venue = product.Venue;
                p.Detail = product.Detail;
                p.Note = product.Note;
                p.Price = product.Price;
                p.image_url = product.image_url;
                p.Status = product.Status;
                p.Host_name = product.Host_name;
                p.UserId = product.UserId;
                return View(p);
            }
            return View(product);
        }

        public IActionResult ProfileHost(int UserId)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == UserId);
            if (user == null)
            {
                TempData["Message"] = "Tài khoản không tồn tại";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }

            var packages = _dbContext.Packages.Where(p => p.UserId == UserId).ToList(); // Lấy danh sách package của user

            var viewModel = new ProfileViewModel
            {
                User = user,
                Packages = packages
            };

            return View(viewModel);
        }
    }
}
