using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace BookingBirthday.Server.Controllers
{
    public class HostThemeController : HostBaseController
    {
        private readonly BookingDbContext _dbContext;
        public HostThemeController(BookingDbContext dbContext)
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

            var theme = _dbContext.Themes.OrderByDescending(x => x.Id).Where(x=>x.Status == "Active").ToList();
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Theme> lst = new PagedList<Theme>(theme, pageNumber, pageSize);
            return View(lst);
        }

        [HttpPost]
        public IActionResult Create(ThemeModel themeModel)
        {
            try
            {

                //var theme = _dbContext.Locations.Where(x => x.Name == themeModel.Name);
                //if (theme.Count() > 0)
                //{
                //    TempData["Message"] = "Chủ đề đã tồn tại";
                //    TempData["Success"] = false;
                //    return RedirectToAction("Index");
                //}

                //thêm đk
                if (themeModel.Name.Length > 30)
                {
                    TempData["Message"] = "Chủ đề không vượt quá 30 kí tự";
                    TempData["Success"] = false;
                    return RedirectToAction("Index");
                }

                var loc = new Data.Entities.Theme();
                loc.UserId = themeModel.UserId;
                loc.Name = themeModel.Name;
                _dbContext.Themes.Add(loc);
                _dbContext.SaveChanges();
                TempData["Message"] = "Thêm mới chủ đề thành công";
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
        public IActionResult Edit(ThemeModel themeModel)
        {
            try
            {

                var theme = _dbContext.Themes.SingleOrDefault(x => x.Id == themeModel.Id);
                if (theme == null)
                {
                    TempData["Message"] = "Chủ đề không tồn tại";
                    TempData["Success"] = false;
                    return View(themeModel);
                }
                //if (theme.Name != themeModel.Name)
                //{
                //    var usr = _dbContext.Themes.Where(x => x.Name == themeModel.Name);
                //    if (usr.Count() > 0)
                //    {
                //        TempData["Message"] = "Chủ đề đã tồn tại";
                //        TempData["Success"] = false;
                //        return RedirectToAction("Index");
                //    }
                //}

                theme.Status = "Active";

                theme.Name = themeModel!.Name;
                
                _dbContext.SaveChanges();
                TempData["Message"] = "Chỉnh sửa thông tin chủ đề thành công";
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
                    TempData["Message"] = "Xóa chủ đề thành công";
                    TempData["Success"] = true;
                }
                else
                {
                    TempData["Message"] = "Xóa chủ đề không thành công";
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
