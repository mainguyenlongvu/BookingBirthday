using Common;
using Data.Services;
using System.Collections.Generic;
using System.Web.Mvc;
using TGClothes.Models;

namespace TGClothes.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly ISlideService _slideService;
        private readonly IProductService _productService;
        private readonly INewsCategoryService _categoryService;
        private readonly IProductCategoryService _productCategoryService;

        public HomeController(
            IMenuService menuService, 
            ISlideService slideService, 
            IProductService productService,
            INewsCategoryService categoryService,
            IProductCategoryService productCategoryService)
        {
            _menuService = menuService;
            _slideService = slideService;
            _productService = productService;
            _categoryService = categoryService;
            _productCategoryService = productCategoryService;
        }

        // GET: Home
        public ActionResult Index()
        {
            var model = _productCategoryService.GetAll();
            ViewBag.Slides = _slideService.GetAll();
            ViewBag.NewProducts = _productService.ListNewProduct(8);
            //ViewBag.FeatureProducts = _productService.ListFeatureProduct(8);
            ViewBag.SaleProducts = _productService.ListSaleProduct(8);
            ViewBag.TopSeller = _productService.ListTopProduct(8);
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var model = _menuService.GetByGroupId(1);
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstants.CART_SESSION];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            var model = _productCategoryService.GetAll();
            ViewBag.Category = _categoryService.GetAll();
            return PartialView(model);
        }
    }
}