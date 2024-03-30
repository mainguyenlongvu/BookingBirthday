using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Common;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace BookingBirthday.Server.Controllers
{
    public class AdminLocationController : AdminBaseController
    {
        private readonly BookingDbContext _dbContext;
        public AdminLocationController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
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

            var location = _dbContext.Locations.OrderByDescending(x => x.Id).ToList();
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Location> lst = new PagedList<Location>(location, pageNumber, pageSize);
            return View(lst);
        }

        [HttpPost]
        public IActionResult Create(LocationModel locationModel)
        {
            try
            {
                
                var locations = _dbContext.Locations.Where(x =>  x.Address == locationModel.Address);
                if (locations.Count() > 0)
                {
                    TempData["Message"] = "Tên địa chỉ đã tồn tại";
                    TempData["Success"] = false;
                    return RedirectToAction("Index");
                }
                var loc = new Data.Entities.Location();
                loc.Address = locationModel.Address;
                loc.AreaId = locationModel.AreaId;
                loc.Name = locationModel.Name;
                _dbContext.Locations.Add(loc);
                _dbContext.SaveChanges();
                TempData["Message"] = "Thêm mới địa điểm thành công";
                TempData["Success"] = true;
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Lỗi tạo địa điểm";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Edit(LocationModel locationModel)
        {
            try
            {
               
                var location = _dbContext.Locations.SingleOrDefault(x => x.Id == locationModel.Id);
                if (location == null)
                {
                    TempData["Message"] = "Địa chỉ không tồn tại";
                    TempData["Success"] = false;
                    return View(locationModel);
                }
                //if (location.Name != locationModel.Name)
                //{
                //    var usr = _dbContext.Locations.Where(x => x.Name == locationModel.Name);
                //    if (usr.Count() > 0)
                //    {
                //        TempData["Message"] = "Tên nhà hàng đã tồn tại";
                //        TempData["Success"] = false;
                //        return RedirectToAction("Index");
                //    }
                //}
                if (location.Address != locationModel.Address)
                {
                    var usr = _dbContext.Locations.Where(x => x.Address == locationModel.Address);
                    if (usr.Count() > 0)
                    {
                        TempData["Message"] = "Địa chỉ nhà hàng đã tồn tại";
                        TempData["Success"] = false;
                        return RedirectToAction("Index");
                    }
                }

                if(locationModel.Address.Length >200)
                {
                    TempData["Message"] = "Địa chỉ không vượt quá 200 kí tự";
                    TempData["Success"] = false;
                    return RedirectToAction("Index");
                }
                location.Status = locationModel.Status;

                if (locationModel.Status == "InActive")
                {
                    var packagesToUpdate = (from a in _dbContext.Packages
                                            join b in _dbContext.Users on a.UserId equals b.Id
                                            where a.UserId == b.Id && b.Status == "Active"
                                            select a).ToList();

                    foreach (var package in packagesToUpdate)
                    {
                        package.Status = "InActive";
                    }

                    _dbContext.SaveChanges();
                }
                else if (locationModel.Status == "Active")

                {
                    var packagesToUpdate = (from a in _dbContext.Packages
                                            join b in _dbContext.Users on a.UserId equals b.Id
                                            where a.UserId == b.Id && b.Status == "InActive"
                                            select a).ToList();

                    foreach (var package in packagesToUpdate)
                    {
                        package.Status = "Active";
                    }

                    _dbContext.SaveChanges();
                }
                location.Address = locationModel.Address;
                location.Name = locationModel.Name;

                _dbContext.SaveChanges();
                TempData["Message"] = "Chỉnh sửa thông tin địa chỉ thành công";
                TempData["Success"] = true;
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Lỗi";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Delete(int userId)
        {
            try
            {
                var user = _dbContext.Locations.Find(userId);
                if (user != null)
                {
                    _dbContext.Locations.Remove(user);
                    _dbContext.SaveChanges();
                    TempData["Message"] = "Xóa địa chỉ thành công";
                    TempData["Success"] = true;
                }
                else
                {
                    TempData["Message"] = "Xóa địa chỉ không thành công";
                    TempData["Success"] = false;
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Lỗi";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }
        }
    }
}
