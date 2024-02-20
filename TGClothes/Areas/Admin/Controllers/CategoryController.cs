using Data.EF;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TGClothes.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly INewsCategoryService _categoryService;

        public CategoryController(INewsCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Admin/Category
        public ActionResult Index()
        {
            var model = _categoryService.GetAll();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                long id = _categoryService.Insert(category);
                if (id > 0)
                {
                    SetAlert("Thêm mới danh mục tin tức thành công", "success");
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm mới danh mục tin tức không thành công.");
                }
            }
            return View("Index");
        }

        public ActionResult Edit(long id)
        {
            var model = _categoryService.GetById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryService.Update(category);
                if (result)
                {
                    SetAlert("Cập nhật danh mục tin tức thành công", "success");
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật danh mục tin tức không thành công");
                }
            }
            return View("Index");
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            _categoryService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = _categoryService.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}