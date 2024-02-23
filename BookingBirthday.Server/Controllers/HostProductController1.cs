using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace BookingBirthday.Server.Controllers
{
    public class HostProductController1 : HostBaseController
    {
        private readonly BookingDbContext _dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string _imageContentFolder;

        public HostProductController1(BookingDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            _imageContentFolder = Path.Combine(webHostEnvironment.WebRootPath, "imgProduct");
        }
        public IActionResult Index()
        {
            var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
            var session = HttpContext.Session;
            List<Category_requests> category_request = null!;

            category_request = _dbContext.Category_Requests
                    .Where(x => x.is_deleted_by_owner == false && x.requester_id == user_id && (x.is_approved == -1 || x.is_approved == 1))
                    .OrderByDescending(x => x.created_at)
                    .ToList();

            if (category_request != null)
            {
                var jsonNotification = JsonConvert.SerializeObject(category_request);
                session.SetString("notification", jsonNotification);
            }

            var products = from a in _dbContext.Packages.Include(x => x.Service)
                           where a.Id == user_id
                           select new { a };
            if (products != null)
            {
                var lstProducts = products.OrderByDescending(x => x.a.Id).Select(x => new PackageModel()
                {
                    Id = x.a.Id,
                    Name = x.a.Name,
                    Detail = x.a.Detail,
                    Venue = x.a.Venue,
                    ServiceId = x.a.ServiceId,
                    Service_Name = x.a.Service!.Name,
                    Price = x.a.Price,
                    PromotionId = x.a.PromotionId,
                    image_url = x.a.image_url
                }).ToList();
                ViewBag.Categories = _dbContext.Packages.ToList();
                return View(lstProducts);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Create(PackageModel productData)
        {
            var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
            var product = _dbContext.Packages.SingleOrDefault(x => x.Name == productData.Name);
            if (product != null)
            {
                TempData["Message"] = "Sản phẩm đã tồn tại";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
            try
            {
                var p = new Package();
                p.Id = user_id;
                p.Name = productData.Name;
                p.Detail = productData.Detail;
                p.ServiceId = productData.ServiceId;
                p.Price = productData.Price;
                p.PromotionId = productData.PromotionId;
                p.Venue = productData.Venue;
                p.image_url = UploadedFile(productData.file!);
                _dbContext.Packages.Add(p);
                _dbContext.SaveChanges();
                TempData["Message"] = "Thêm mới sản phẩm thành công";
                TempData["Success"] = true;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Lỗi";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Edit(PackageModel productData)
        {
            var p = _dbContext.Packages.SingleOrDefault(x => x.Id == productData.Id);
            if (p == null)
            {
                TempData["Message"] = "Sản phẩm không tồn tại";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
            try
            {
                p.Name = productData.Name;
                p.Detail = productData.Detail;
                p.ServiceId = productData.ServiceId;
                p.Price = productData.Price;
                p.PromotionId = productData.PromotionId;
                p.Venue = productData.Venue;
                if (productData.file != null)
                {
                    if (p.image_url != "/imgProduct/" && p.image_url != null)
                    {
                        var n = p.image_url!.Remove(0, 12);
                        DeleteImage(n);
                    }
                    p.image_url = UploadedFile(productData.file!);
                }
                _dbContext.SaveChanges();
                TempData["Message"] = "Chỉnh sửa thông tin sản phẩm thành công";
                TempData["Success"] = true;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Lỗi";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Delete(int productId)
        {
            try
            {
                var product = _dbContext.Packages.Find(productId);
                if (product != null)
                {
                    if (product.image_url != "/imgProduct/" && product.image_url != null)
                    {
                        var n = product.image_url!.Remove(0, 12);
                        DeleteImage(n);
                    }
                    _dbContext.Packages.Remove(product);
                    _dbContext.SaveChanges();
                    TempData["Message"] = "Xóa sản phẩm thành công";
                    TempData["Success"] = true;
                }
                else
                {
                    TempData["Message"] = "Xóa sản phẩm không thành công";
                    TempData["Success"] = false;
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
        private string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null!;

            if (file != null)
            {
                string uploadsFolder = _imageContentFolder;
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return "/imgProduct/" + uniqueFileName!;
        }
        public void DeleteImage(string fileName)
        {
            var filePath = Path.Combine(_imageContentFolder, fileName);
            if (System.IO.File.Exists(filePath))
            {
                Task.Run(() => System.IO.File.Delete(filePath));
            }
        }
    }
}
