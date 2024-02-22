﻿using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookingBirthday.Server.Controllers
{
    public class AdminServiceController : AdminBaseController
    {
        private readonly BookingDbContext _dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string _imageContentFolder;

        public AdminServiceController(BookingDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            _imageContentFolder = Path.Combine(webHostEnvironment.WebRootPath, "imgProduct");
        }
        public IActionResult Index()
        {
            var session = HttpContext.Session;
            List<Category_requests> category_request = null!;

            category_request = _dbContext.Category_Requests
                    .Where(x => x.is_approved == 0 && x.is_deleted_by_admin == false)
                    .OrderByDescending(x => x.created_at)
                    .ToList();

            if (category_request != null)
            {
                var jsonNotification = JsonConvert.SerializeObject(category_request);
                session.SetString("notification", jsonNotification);
            }

            var products = from a in _dbContext.Services
                           select new { a };
            if (products != null)
            {
                var lstProducts = products.OrderByDescending(x => x.a.Id).Select(x => new ServiceModel()
                {
                    Id = x.a.Id,
                    Name = x.a.Name,
                    Detail = x.a.Detail,
                    PromotionId = x.a.PromotionId,
                    Price = x.a.Price,
                    Venue = x.a.Venue,
                    image_url = x.a.image_url
                }).ToList();
                ViewBag.Categories = _dbContext.Services.ToList();
                return View(lstProducts);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Create(ServiceModel productData)
        {
            var product = _dbContext.Services.SingleOrDefault(x => x.Name == productData.Name);
            if (product != null)
            {
                TempData["Message"] = "Sản phẩm đã tồn tại";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
            try
            {
                var p = new Service();
                p.Name = productData.Name;
                p.Detail = productData.Detail;
                p.PromotionId = productData.PromotionId;
                p.Price = productData.Price;
                p.Venue = productData.Venue;
                p.image_url = UploadedFile(productData.file!);
                _dbContext.Services.Add(p);
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
        public IActionResult Edit(ServiceModel productData)
        {
            var p = _dbContext.Services.SingleOrDefault(x => x.Id == productData.Id);
            if (p == null)
            {
                TempData["Message"] = "Sản phẩm không tồn tại";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
            try
            {
                var pa = new Service();
                pa.Name = productData.Name;
                pa.Detail = productData.Detail;
                pa.PromotionId = productData.PromotionId;
                pa.Price = productData.Price;
                pa.Venue = productData.Venue;
                if (productData.file != null)
                {
                    if (pa.image_url != "/imgProduct/" && pa.image_url != null)
                    {
                        var n = pa.image_url!.Remove(0, 10);
                        DeleteImage(n);
                    }
                    pa.image_url = UploadedFile(productData.file!);
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
                var product = _dbContext.Services.Find(productId);
                if (product != null)
                {
                    if (product.image_url != "/imgProduct/" && product.image_url != null)
                    {
                        var n = product.image_url!.Remove(0, 12);
                        DeleteImage(n);
                    }
                    _dbContext.Services.Remove(product);
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