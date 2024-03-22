using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
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

            var product = _dbContext.Packages.Include(x => x.Category).Where(x => x.Id == Id).FirstOrDefault();
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

