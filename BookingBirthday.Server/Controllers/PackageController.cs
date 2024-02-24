using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace BookingBirthday.Server.Controllers
{
    public class PackageController : Controller
    {
        private readonly BookingDbContext _dbContext;

        public PackageController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Show(int productId)
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

            var product = _dbContext.Packages.Where(x => x.Id == productId).FirstOrDefault();
            if (product != null)
            {
                var p = new PackageModel();
                p.Id = product.Id;
                p.Name = product.Name;
                p.Name = product.Name;
                p.Venue = product.Venue;
                p.PromotionId = product.PromotionId;
                p.Price = product.Price;
                p.image_url = product.image_url;
                return View(p);
            }
            return View(product);
        }
    }
}
