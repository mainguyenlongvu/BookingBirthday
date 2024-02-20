using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TGClothes.Controllers
{
    public class ContentController : Controller
    {
        private readonly INewsService _contentService;
        private readonly INewsCategoryService _categoryService;

        public ContentController(INewsService contentService, INewsCategoryService categoryService)
        {
            _contentService = contentService;
            _categoryService = categoryService;
        }
        // GET: Content
        public ActionResult Index(int page = 1, int pageSize = 4)
        {
            int totalRecord = 0;
            var model = _contentService.GetAll();
            totalRecord = model.Count();

            var result = model.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            int maxPage = 5;
            int totalPage = 0;

            ViewBag.TotalRecord = totalRecord;
            ViewBag.Page = page;
            totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(result);
        }

        [ValidateInput(false)]
        public ActionResult Detail(long id)
        {
            var model = _contentService.GetById(id);
            ViewBag.Tags = _contentService.ListTag(id);
            return View(model);
        }

        public ActionResult Tag(string tagId, int page = 1, int pageSize = 4)
        {
            var model = _contentService.GetAllByTag(tagId, page, pageSize);
            int totalRecord = 0;
            totalRecord = _contentService.GetAllByTag(tagId, page, pageSize).Count();
            int maxPage = 5;
            ViewBag.TotalRecord = totalRecord;
            int totalPage = 0;
            ViewBag.Page = page;
            ViewBag.Tag = _contentService.GetTag(tagId);
            totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(model);
        }

        public ActionResult Category(long categoryId, int page = 1, int pageSize = 4)
        {
            var model = _contentService.GetAllByCategory(categoryId, page, pageSize);
            int totalRecord = 0;
            totalRecord = _contentService.GetAllByCategory(categoryId, page, pageSize).Count();
            int maxPage = 5;
            ViewBag.TotalRecord = totalRecord;
            int totalPage = 0;
            ViewBag.Page = page;
            ViewBag.Category = _contentService.GetCategory(categoryId);
            totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult ContentCategory()
        {
            var model = _categoryService.GetAll();
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult ContentTag()
        {
            var model = _contentService.GetAllTag();
            return PartialView(model);
        }
    }
}