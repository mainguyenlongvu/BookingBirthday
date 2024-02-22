using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace BookingBirthday.Server.Controllers
{
    public class HomeController : Controller
    {

        private readonly BookingDbContext _dbContext;
        public HomeController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
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
                else if (role == "Store Owner")
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


            var products = from a in _dbContext.Packages.Include(x => x.Service)
                           select new { a };
            if (products != null)
            {
                var lstProducts = products.OrderByDescending(x => x.a.Id).Select(x => new PackageModel()
                {
                    Id = x.a.Id,
                    Name = x.a.Name,
                    Detail = x.a.Detail,
                    PromotionId = x.a.PromotionId,
                    ServiceId = x.a.ServiceId,
                    Service_Name = x.a.Service!.Name,
                    Price = x.a.Price,
                    Venue = x.a.Venue,
                    image_url = x.a.image_url
                }).ToList();
                ViewBag.Categories = _dbContext.Packages.ToList();
                return View(lstProducts);
            }
            return View();
        }
        public IActionResult FilterProducts(int ServiceId)
        {
            // Logic filter danh sách sản phẩm theo category
            var filteredProducts = from a in _dbContext.Packages.Include(x => x.Service)
                                   select new { a };
            if (ServiceId != 0)
            {
                filteredProducts = filteredProducts.Where(x => x.a.ServiceId == ServiceId);
            }
            if (filteredProducts != null)
            {
                var lstProducts = filteredProducts.Select(x => new PackageModel()
                {
                    Id = x.a.Id,
                    Name = x.a.Name,
                    Detail = x.a.Detail,
                    Venue = x.a.Venue,
                    ServiceId = x.a.ServiceId,
                    PromotionId = x.a.PromotionId,
                    Service_Name = x.a.Service!.Name,
                    Price = x.a.Price,
                    image_url = x.a.image_url
                }).ToList();
                return PartialView("ProductList", lstProducts);
            }
            return PartialView("ProductList", null);
        }
        public IActionResult Search(string keyword)
        {
            var filteredProducts = from a in _dbContext.Packages.Include(x => x.Service)
                                   select new { a };

            filteredProducts = filteredProducts.Where(x => x.a.Name!.Contains(keyword));
            if (filteredProducts != null)
            {
                var lstProducts = filteredProducts.Select(x => new PackageModel()
                {
                    Id = x.a.Id,
                    Name = x.a.Name,
                    Detail = x.a.Detail,
                    Venue = x.a.Venue,
                    ServiceId = x.a.ServiceId,
                    PromotionId = x.a.PromotionId,
                    Service_Name = x.a.Service!.Name,
                    Price = x.a.Price,
                    image_url = x.a.image_url
                }).ToList();
                return View(lstProducts);
            }
            return View(filteredProducts);
        }
    }
}