﻿using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using X.PagedList;

namespace BookingBirthday.Server.Controllers
{
    public class AdminPackageController : AdminBaseController
    {
        private readonly BookingDbContext _dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string _imageContentFolder;

        public AdminPackageController(BookingDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            _imageContentFolder = Path.Combine(webHostEnvironment.WebRootPath, "imgPackage");
        }
        public IActionResult Index(int? page)
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

            var products = _dbContext.Packages
                    .Include(p => p.PackageLocations)
                        .ThenInclude(pl => pl.Location)
                    .ToList();
            if (products != null)
            {
                var lstProducts = products.OrderByDescending(x => x.Id).Select(x => new PackageModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Detail = x.Detail,
                    Price = x.Price,
                    Note = x.Note,
                    Host_name = x.Host_name,
                    image_url = x.image_url,
                    PackageType = x.PackageType,
                    PackageLocations = x.PackageLocations,
                    ThemeId = x.ThemeId,
                    Gender = x.Gender,
                    Age = x.Age,
                    Theme = x.Theme,
                    Status = x.Status,
                    UserId = x.UserId,
                    Locations = x.PackageLocations.Select(pl => pl.Location).ToList(),

                }).ToList();
                int pageSize = 8;
                int pageNumber = page == null || page < 0 ? 1 : page.Value;
                PagedList<PackageModel> lst = new PagedList<PackageModel>(lstProducts, pageNumber, pageSize);
                return View(lst);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Create(PackageModel productData)
        {
            var product = _dbContext.Packages.SingleOrDefault(x => x.Name == productData.Name);
            if (product != null)
            {
                TempData["Message"] = "Bữa tiệc đã tồn tại";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
            try
            {
                var p = new Package();
                p.Name = productData.Name;
                p.Detail = productData.Detail;
                p.Price = productData.Price;
                p.image_url = UploadedFile(productData.file!);
                _dbContext.Packages.Add(p);
                _dbContext.SaveChanges();
                TempData["Message"] = "Thêm mới bữa tiệc thành công";
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
                TempData["Message"] = "Bữa tiệc không tồn tại";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
            try
            {
                var pa = new Package();
                pa.Name = productData.Name;
                pa.Detail = productData.Detail;
                pa.Price = productData.Price;
                if (productData.file != null)
                {
                    if (pa.image_url != "/imgPackage/" && pa.image_url != null)
                    {
                        var n = pa.image_url!.Remove(0, 10);
                        DeleteImage(n);
                    }
                    pa.image_url = UploadedFile(productData.file!);
                }
                _dbContext.SaveChanges();
                TempData["Message"] = "Chỉnh sửa thông tin bữa tiệc thành công";
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
                    if (product.image_url != "/imgPackage/" && product.image_url != null)
                    {
                        var n = product.image_url!.Remove(0, 12);
                        DeleteImage(n);
                    }
                    _dbContext.Packages.Remove(product);
                    _dbContext.SaveChanges();
                    TempData["Message"] = "Xóa bữa tiệc thành công";
                    TempData["Success"] = true;
                }
                else
                {
                    TempData["Message"] = "Xóa bữa tiệc không thành công";
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
            return "/imgPackage/" + uniqueFileName!;
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
