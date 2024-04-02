using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using X.PagedList;

namespace BookingBirthday.Server.Controllers
{
    public class HomeController : Controller
    {

        private readonly BookingDbContext _dbContext;
        public HomeController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(int? page)
        {
            var session = HttpContext.Session;
            var role = HttpContext.Session.GetString("role");
            List<Category_requests> category_request = null!;

            if (role != null)
            {

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
            }
            int pageSize = 8;
            int pageNumber = page ?? 1;
            //var lstsanpham = _dbContext.Packages.Include(x => x.Category).AsNoTracking().OrderBy(x => x.Id);

            var products = from a in _dbContext.Packages
                           select new { a };
            if (products != null)
            {
                var lstProducts = products.OrderByDescending(x => x.a.Id).Select(x => new PackageModel()
                {
                    Id = x.a.Id,
                    Name = x.a.Name,
                    Detail = x.a.Detail,
                    Price = x.a.Price,
                    Note = x.a.Note,
                    image_url = x.a.image_url,
                    Status = x.a.Status,
                    Host_name = x.a.Host_name,
                    UserId = x.a.UserId,
                }).ToList();
                // Convert the list to a paged list
                var pagedList = lstProducts.ToPagedList(pageNumber, pageSize);
                return View(pagedList);
            }
            return View();
        }
        //public IActionResult FilterProducts(int category_id, int? page)
        //{
        //    // Logic filter danh sách sản phẩm theo category
        //    var filteredProducts = from a in _dbContext.Packages
        //                           select new { a };

        //    if (category_id != 0)
        //    {
        //        filteredProducts = filteredProducts.Where(x => x.a.category_id == category_id);
        //    }

        //    var lstProducts = filteredProducts.Select(x => new PackageModel()
        //    {
        //        Id = x.a.Id,
        //        Name = x.a.Name,
        //        Detail = x.a.Detail,
        //        Venue = x.a.Venue,
        //        Price = x.a.Price,
        //        Note = x.a.Note,
        //        image_url = x.a.image_url,
        //        Status = x.a.Status,
        //        UserId = x.a.UserId,
        //        Host_name = x.a.Host_name,
        //    }).ToList();
        //    int pageSize = 8;
        //    int pageNumber = page == null || page < 0 ? 1 : page.Value;
        //    PagedList<PackageModel> lst = new PagedList<PackageModel>(lstProducts, pageNumber, pageSize);

        //        return PartialView("PackageList", lst);

        //}


        public IActionResult Search(string keyword)
        {
            var filteredProducts = from a in _dbContext.Packages
                                   select new { a };

            filteredProducts = filteredProducts.Where(x => x.a.Name!.Contains(keyword) || x.a.Host_name!.Contains(keyword));
            if (filteredProducts != null)
            {
                var lstProducts = filteredProducts.Select(x => new PackageModel()
                {
                    Id = x.a.Id,
                    Name = x.a.Name,
                    Detail = x.a.Detail,
                    Price = x.a.Price,
                    Note = x.a.Note,
                    image_url = x.a.image_url,
                    Host_name = x.a.Host_name,
                    Status = x.a.Status,
                    UserId = x.a.UserId,
                }).ToList();
                return View(lstProducts);
            }
            return View(filteredProducts);
        }

        [HttpGet]
        public IActionResult FilterPackages(string category, string details)
        {
            IQueryable<Package> filteredPackages = _dbContext.Packages;

            if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(details))
            {
                switch (category)
                {
                    case "gender":
                        filteredPackages = filteredPackages.Where(p => p.Gender == details);
                        break;
                    case "age":
                        filteredPackages = filteredPackages.Where(p => p.Age == details);
                        break;
                    case "packageType":
                        filteredPackages = filteredPackages.Where(p => p.PackageType == details);
                        break;
                    case "theme":
                        filteredPackages = filteredPackages.Where(p => p.Theme.Name == details);
                        break;
                    case "area":
                        filteredPackages = filteredPackages
                            .Join(_dbContext.PackageLocations,
                                p => p.Id,
                                pl => pl.PackageId,
                                (p, pl) => new { Package = p, PackageLocation = pl })
                            .Join(_dbContext.Locations,
                                pl => pl.PackageLocation.LocationId,
                                l => l.Id,
                                (pl, l) => new { pl.Package, Location = l })
                            .Where(x => x.Location.Area.Name == details)
                            .Select(x => x.Package)
                            .Distinct();
                        break;

                }
            }

            var pageNumber = 1; // Default page number
            var pageSize = 8; // Default page size

            var pagedFilteredPackages = filteredPackages.ToPagedList(pageNumber, pageSize);

            var filteredPackageModels = pagedFilteredPackages.Select(p => new PackageModel
            {
                Id = p.Id,
                Name = p.Name,
                Detail = p.Detail,
                Price = p.Price,
                Note = p.Note,
                image_url = p.image_url,
                Host_name = p.Host_name,
                Status = p.Status,
                UserId = p.UserId,
            }).ToPagedList(pageNumber, pageSize); // Convert to paged list of PackageModel

            return PartialView("PackageList", filteredPackageModels);
        }

    }
}