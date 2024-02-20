using Data.EF;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TGClothes.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService _productCategory;

        public ProductCategoryController(IProductCategoryService productCategory)
        {
            _productCategory = productCategory;
        }
        // GET: Admin/ProductCategory
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = _productCategory.GetAllPaging(page, pageSize);
            TempData["ParentCategories"] = _productCategory.GetAll().Where(x => x.ParentId == null).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new ProductCategory
            {
                ParentCategories = GetAllCategories()
            };
            return View(model);
        }

        private IEnumerable<ProductCategory> GetAllCategories()
        {
            var items = _productCategory.GetAll();

            var parentIds = items.Select(i => i.ParentId).Distinct().Where(id => id.HasValue).Select(id => id.Value);

            var topLevelItems = items.Where(i => i.ParentId == null);

            return topLevelItems;
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (string.IsNullOrEmpty(productCategory.Name))
            {
                ViewData["Error"] = "Vui lòng nhập tên danh mục!";
            }
            else
            {
                if (!productCategory.Status) // Nếu trường status không được gửi từ form (giá trị mặc định là false)
                {
                    productCategory.Status = true; // Gán giá trị mặc định là true
                }

                long id = _productCategory.Insert(productCategory);
                if (id > 0)
                {
                    SetAlert("Thêm mới danh mục sản phẩm thành công", "success");
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm mới danh mục sản phẩm không thành công.");
                }
            }

            return View("Index");
        }

        public ActionResult Edit(long id)
        {
            var productCategory = _productCategory.GetProductCategoryById(id);
            productCategory.ParentCategories = GetAllCategories();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                ViewData["Error"] = "Vui lòng nhập tên danh mục!";
            }
            else
            {
                if (!model.Status) // Nếu trường status không được gửi từ form (giá trị mặc định là false)
                {
                    model.Status = true; // Gán giá trị mặc định là true
                }

                bool success = _productCategory.Update(model);
                if (success)
                {
                    SetAlert("Chỉnh sửa danh mục sản phẩm thành công", "success");
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Chỉnh sửa danh mục sản phẩm không thành công.");
                }
            }

            // Nếu có lỗi xảy ra hoặc model không hợp lệ, trả về view "Edit" với model để hiển thị lại form
            model.ParentCategories = GetAllCategories();
            return View(model);
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            _productCategory.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = _productCategory.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpGet]
        public ActionResult GetParentCategories()
        {
            // Lấy danh sách danh mục cha từ CSDL
            var parentCategories = _productCategory.GetAll().Where(x => x.ParentId == null).ToList();

            // Trả về kết quả dưới dạng JSON
            return Json(parentCategories, JsonRequestBehavior.AllowGet);
        }

        // Action để trả về danh sách danh mục con của một danh mục cha
        [HttpGet]
        public ActionResult GetChildCategories(int parentId)
        {
            // Lấy danh sách danh mục con của danh mục cha từ CSDL
            var childCategories = _productCategory.GetAll().Where(c => c.ParentId == parentId).ToList();

            // Trả về kết quả dưới dạng JSON
            return Json(childCategories, JsonRequestBehavior.AllowGet);
        }
    }
}