using Data.EF;
using Data.Services;
using System.Web.Mvc;

namespace TGClothes.Areas.Admin.Controllers
{
    public class SizeController : BaseController
    {
        private readonly ISizeService _sizeService;

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }
        // GET: Admin/Size
        public ActionResult Index()
        {
            var sizes = _sizeService.GetAll();
            return View(sizes);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Size size)
        {
            if (ModelState.IsValid)
            {
                long id = _sizeService.Insert(size);
                if (id > 0)
                {
                    SetAlert("Thêm mới size thành công", "success");
                    return RedirectToAction("Index", "Size");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm mới size không thành công.");
                }
            }
            return View("Index");
        }

        public ActionResult Edit(long id)
        {
            var size = _sizeService.GetSizeById(id);
            return View(size);
        }

        [HttpPost]
        public ActionResult Edit(Size size)
        {
            if (ModelState.IsValid)
            {
                var result = _sizeService.Update(size);
                if (result)
                {
                    SetAlert("Cập nhật size thành công", "success");
                    return RedirectToAction("Index", "Size");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật size không thành công");
                }
            }
            return View("Index");
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            _sizeService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}