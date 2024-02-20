using Data.EF;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TGClothes.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        private readonly ISlideService _slideService;

        public SlideController(ISlideService slideService)
        {
            _slideService = slideService;
        }

        // GET: Admin/Slide
        public ActionResult Index()
        {
            var slides = _slideService.GetAll();
            return View(slides);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Slide slide)
        {
            if (ModelState.IsValid)
            {
                long id = _slideService.Insert(slide);
                if (id > 0)
                {
                    SetAlert("Thêm mới slide thành công", "success");
                    return RedirectToAction("Index", "Slide");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm mới slide không thành công.");
                }
            }
            return View("Index");
        }

        public ActionResult Edit(long id)
        {
            var slide = _slideService.GetSlideById(id);
            return View(slide);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Slide slide)
        {
            if (ModelState.IsValid)
            {
                var result = _slideService.Update(slide);
                if (result)
                {
                    SetAlert("Cập nhật slide thành công", "success");
                    return RedirectToAction("Index", "Slide");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật slide không thành công");
                }
            }
            return View("Index");
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            _slideService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = _slideService.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}