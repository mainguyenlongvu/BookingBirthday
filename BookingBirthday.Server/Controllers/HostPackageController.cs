using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Controllers;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    .Include(p => p.PackageLocations)
                        .ThenInclude(pl => pl.Location)
                    .ToList();
        //var location = _dbContext.Locations.ToList();
        //var area = _dbContext.Areas.ToList();
        //var viewModel = new PackageModel
        //{
        //    Locations = location,
        //    Areas = area,
        //    Packages = products
        //};
        if (products != null)
        {
            var lstProducts = products.Where(x => x.UserId == user_id).OrderByDescending(x => x.Id).Where(x => x.Status == "Active").Select(x => new PackageModel()
            {
                Id = x.Id,
                Name = x.Name,
                Detail = x.Detail,
                Note = x.Note,
                Price = x.Price,
                image_url = x.image_url,
                Host_name = x.Host_name,
                Status = x.Status,
                PackageType = x.PackageType,
                Age = x.Age,
                ThemeId = x.ThemeId,
                PackageLocations = x.PackageLocations,
                Gender  = x.Gender,
                UserId = x.UserId,
                Locations = x.PackageLocations.Select(pl => pl.Location).ToList(),

            });
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<PackageModel> lst = new PagedList<PackageModel>(lstProducts, pageNumber, pageSize);
            return View(lst);
        }
        return View();
    }

    [HttpPost]
    public IActionResult Create( PackageModel productData)
    {
        var user_id = int.Parse(HttpContext.Session.GetString("user_id")!);
        if (productData!.Price > 1000000 || productData!.Price < 100000)
        {
            TempData["Message"] = "Giá gói tiệc phải nằm trong khoảng từ 100k đến 1 triệu";
            TempData["Success"] = false;
            return RedirectToAction("Index");
        }
        try
        {
            var address = productData.selectedAddresses.ToList();
            var location = _dbContext.Locations.FirstOrDefault(x => x.Address == address[0]);
            var area = _dbContext.Areas.FirstOrDefault(x => x.Id == location.AreaId);
            var theme = _dbContext.Themes.FirstOrDefault(x => x.Id == productData.ThemeId);
            var product = "Gói tiệc " + productData.PackageType + " Chủ đề " + theme.Name + " " + area.Name;
            var check = _dbContext.Locations.FirstOrDefault(x => x.Address == product);
            if (check != null)
            {
                TempData["Message"] = "Gói tiệc đã tồn tại";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
          var  p = new Package
            {
                
                UserId = user_id,
                Name = "Gói tiệc " + productData.PackageType + " Chủ đề " + theme.Name + " " + area.Name,
                Detail = productData.Detail!,
                Note = productData.Note,
                Price = productData.Price,
                ThemeId = productData.ThemeId,
                PackageType = productData.PackageType,
                Age = productData.Age,
                Gender = productData.Gender,               
                image_url = UploadedFile(productData.file!),
                Status = "Active",
                Host_name = HttpContext.Session.GetString("name")!,
            };
                
            _dbContext.Packages.Add(p);
            _dbContext.SaveChanges();
            foreach (string item in productData.selectedAddresses)
            {
                var locationId = _dbContext.Locations.FirstOrDefault(x => x.Address == item);
                var packagelocation = new PackageLocation();
                packagelocation.PackageId = p.Id;
                packagelocation.LocationId = locationId.Id;
                _dbContext.PackageLocations.Add(packagelocation);
            }
            _dbContext.SaveChanges();//lỗi ờ đây
            
            

            TempData["Message"] = "Thêm mới gói tiệc thành công";
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
            TempData["Message"] = "Gói tiệc không tồn tại";
            TempData["Success"] = false;
            return RedirectToAction("Index");
        }
        try
        {
            var address = productData.selectedAddresses.ToList().First();
            var location = _dbContext.Locations.FirstOrDefault(x => x.Address == address);
            var area = _dbContext.Areas.FirstOrDefault(x => x.Id == location.AreaId);
            var theme = _dbContext.Themes.FirstOrDefault(x => x.Id == productData.ThemeId);
            if(productData.selectedAddresses.Count != null)
            {
                p.Name = "Gói tiệc " + productData.PackageType + " Chủ đề " + theme.Name + " " + area.Name;
            }
            p.Detail = productData.Detail;
            p.Note = productData.Note;
            p.Price = productData.Price;
            p.Age = productData.Age;
            p.PackageType = productData.PackageType;
            p.ThemeId = productData.ThemeId;
            p.Gender = productData.Gender;
            p.Detail = productData.Detail;

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

            if (productData.selectedAddresses.Count != null)
            {
                var remove = _dbContext.PackageLocations.Where(x => x.PackageId == productData.Id).ToList();
                foreach( var item in remove)
                {
                    _dbContext.PackageLocations.Remove(item);
                }
            }
            _dbContext.SaveChanges();



            foreach (string item in productData.selectedAddresses)
            {
                var locationId = _dbContext.Locations.FirstOrDefault(x => x.Address == item);
                var packagelocation = new PackageLocation();
                packagelocation.PackageId = productData.Id;
                packagelocation.LocationId = locationId.Id;
                _dbContext.PackageLocations.Add(packagelocation);
            }
            _dbContext.SaveChanges();//lỗi ờ đây

            TempData["Message"] = "Chỉnh sửa thông tin gói tiệc thành công";
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
                product.Status = "InActive";
                //if (product.image_url != "/imgPackage/" && product.image_url != null)
                //{
                //    var n = product.image_url!.Remove(0, 12);
                //    DeleteImage(n);
                //}
                //Console.WriteLine(product);
                //_dbContext.Packages.Remove(product);
                _dbContext.SaveChanges();
                TempData["Message"] = "Xóa gói tiệc thành công";
                TempData["Success"] = true;
            }
            else
            {
                TempData["Message"] = "Xóa gói tiệc không thành công";
                TempData["Success"] = false;
            }
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Message"] = "Đã xảy ra lỗi khi xóa gói tiệc: " + ex.Message;
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


    [HttpGet]
    public async Task<IActionResult> GetLocationsByAreaId(int areaId)
    {
        var locations = await _dbContext.Locations
                                       .Where(l => l.AreaId == areaId)
                                       .Select(l => new { l.Name })
                                       .Distinct()
                                       .ToListAsync();

        return Json(locations);
    }

    [HttpPost]
    public async Task<IActionResult> GetAddressesByLocationsAndAreaId([FromBody] LocationRequestModel request)
    {
        var addresses = await _dbContext.Locations
                                       .Where(l => request.LocationNames.Contains(l.Name) && l.AreaId == request.AreaId && l.Status == "Active").OrderBy(l=>l.Name)
                                       .Select(l => new { l.Id, l.Address })
                                       .ToListAsync();

        return Json(addresses);
    }

    public async Task<IActionResult> GetAddressesByLocationNameAndAreaId(string locationName, int areaId)
    {
        var addresses = await _dbContext.Locations
                                       .Where(l => l.Name == locationName && l.AreaId == areaId && l.Status == "Active")
                                       .Select(l => new { l.Id, l.Address })
                                       .ToListAsync();

        return Json(addresses);
    }

    //[HttpPost]
    //public IActionResult SaveSelectedAddresses([FromBody] List<string> selectedAddresses)
    //{
    //    // Lưu danh sách địa chỉ đã chọn vào thuộc tính selectedAddresses của Packagemodel
    //    PackageModel.selectedAddresses = selectedAddresses;
    //    // Trả về mã trạng thái HTTP 200 để cho biết lưu dữ liệu thành công
    //    return Ok();
    //}


}

