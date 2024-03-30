using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Controllers;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

public class HostPackageController : HostBaseController
{
    private readonly BookingDbContext _dbContext;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly string _imageContentFolder;
    

    public HostPackageController(BookingDbContext dbContext, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        this.webHostEnvironment = webHostEnvironment;
        _imageContentFolder = Path.Combine(webHostEnvironment.WebRootPath, "imgPackage");
    }

    public IActionResult Index(int? page)
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
        var products = _dbContext.Packages
                       .Where(x => x.UserId == user_id);
                      
        if (products != null)
        {
            var lstProducts = products.Where(x => x.UserId == user_id).OrderByDescending(x => x.Id).Select(x => new PackageModel()
            {
                Id = x.Id,
                Name = x.Name,
                Detail = x.Detail,
                Note = x.Note,
                Price = x.Price,
                image_url = x.image_url,
                Host_name = x.Host_name,
                Status = x.Status,
                UserId = x.UserId
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
        var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
        var product = _dbContext.Packages.FirstOrDefault(x => x.Name == productData.Name);
        if (product != null)
        {
            TempData["Message"] = "Bữa tiệc đã tồn tại";
            TempData["Success"] = false;
            return RedirectToAction("Index");
        }
        if (productData!.Price <500000)
        {
            TempData["Message"] = "Giá bữa tiệc phải trên 500k";
            TempData["Success"] = false;
            return RedirectToAction("Index");
        }
        try
        {
            var p = new Package();
            // Giá từ 500k-1tr
            if (productData.Price >= 500000 && productData.Price < 1000000)
            {
                 p = new Package
                {
                    UserId = user_id,
                    Name = productData.Name!,
                    Detail = productData.Detail!,
                    Note = productData.Note,
                    Price = productData.Price,
                    image_url = UploadedFile(productData.file!),
                    Status = "Active",
                    Host_name = HttpContext.Session.GetString("name")!,
                };
            }
            // Giá từ 1tr-2tr
            if (productData.Price >= 1000000 && productData.Price < 2000000)
            {
                 p = new Package
                {
                    UserId = user_id,
                    Name = productData.Name!,
                    Detail = productData.Detail!,
                    Note = productData.Note,
                    Price = productData.Price,
                    image_url = UploadedFile(productData.file!),
                    Status = "Active",
                    Host_name = HttpContext.Session.GetString("name")!,
                };
            }
            // Giá từ 2tr-3tr
            if (productData.Price >= 2000000 && productData.Price < 3000000)
            {
                 p = new Package
                {
                    UserId = user_id,
                    Name = productData.Name!,
                    Detail = productData.Detail!,
                    Note = productData.Note,
                    Price = productData.Price,
                    image_url = UploadedFile(productData.file!),
                    Status = "Active",
                    Host_name = HttpContext.Session.GetString("name")!,
                };
            }
            // Giá từ 3tr-4tr
            if (productData.Price >= 3000000 && productData.Price < 4000000)
            {
                 p = new Package
                {
                    UserId = user_id,
                    Name = productData.Name!,
                    Detail = productData.Detail!,
                    Note = productData.Note,
                    Price = productData.Price,
                    image_url = UploadedFile(productData.file!),
                    Status = "Active",
                    Host_name = HttpContext.Session.GetString("name")!,
                };
            }
            // Giá từ 4tr-5tr
            if (productData.Price >= 4000000 && productData.Price < 5000000)
            {
                 p = new Package
                {
                    UserId = user_id,
                    Name = productData.Name!,
                    Detail = productData.Detail!,
                    Note = productData.Note,
                    Price = productData.Price,
                    image_url = UploadedFile(productData.file!),
                    Status = "Active",
                    Host_name = HttpContext.Session.GetString("name")!,
                };
            }
            //Giá trên 5tr
            if (productData.Price >= 5000000)
            {
                 p = new Package
                {
                    UserId = user_id,
                    Name = productData.Name!,
                    Detail = productData.Detail!,
                    Note = productData.Note,
                    Price = productData.Price,
                    image_url = UploadedFile(productData.file!),
                    Status = "Active",
                    Host_name = HttpContext.Session.GetString("name")!,
                };
            }

            _dbContext.Packages.Add(p);
            _dbContext.SaveChanges();

            TempData["Message"] = "Thêm mới bữa tiệc thành công";
            TempData["Success"] = true;
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Message"] = "Lỗi: " + ex.Message; // Xử lý ngoại lệ một cách cụ thể
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
            p.Name = productData.Name;
            p.Detail = productData.Detail;
            p.Note = productData.Note;
            p.Price = productData.Price;
            if (productData.file != null)
            {
                if (p.image_url != "/imgPackage/" && p.image_url != null)
                {
                    var n = p.image_url!.Remove(0, 12);
                    DeleteImage(n);
                }
                p.image_url = UploadedFile(productData.file!);
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
    public IActionResult Delete(int Id)
    {
        try
        {
            var product = _dbContext.Packages.Find(Id);
            if (product != null)
            {
                if (product.image_url != "/imgPackage/" && product.image_url != null)
                {
                    var n = product.image_url!.Remove(0, 12);
                    DeleteImage(n);
                }
                Console.WriteLine(product);
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
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Message"] = "Đã xảy ra lỗi khi xóa bữa tiệc: " + ex.Message;
            TempData["Success"] = false;
            return RedirectToAction("Index");
        }
    }

    private string UploadedFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null; // Tránh truy cập vào thuộc tính FileName của null

        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        string filePath = Path.Combine(_imageContentFolder, uniqueFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }
        return "/imgPackage/" + uniqueFileName;
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

