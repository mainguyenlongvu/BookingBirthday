using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data.EF;
using Data.Services;
using Common;
using TGClothes.Models;

namespace TGClothes.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IRateService _rateService;
        private readonly ISizeService _sizeService;
        private readonly IProductStockService _productSizeService;
        private readonly IAccountService _userService;
        private readonly IGalleryService _galleryService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;

        public ProductController(
            IProductService productService, 
            IProductCategoryService productCategoryService, 
            IRateService rateService,
            ISizeService sizeService,
            IProductStockService productSizeService,
            IAccountService userService,
            IGalleryService galleryService, 
            IOrderService orderService,
            IOrderDetailService orderDetailService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _rateService = rateService;
            _sizeService = sizeService;
            _productSizeService = productSizeService;
            _userService = userService;
            _galleryService = galleryService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }

        // GET: Admin/Product
        public ActionResult Index(int page = 1, int pageSize = 8, string orderBy = "")
        {
            int totalRecord = 0;
            int maxPage = 5;
            int totalPage = 0;

            var model = _productService.GetAll(ref totalRecord, page, pageSize);
            var result = (from p in _productService.GetAll().Where(x => x.Status == true)
                          join ps in _productSizeService.GetAll() on p.Id equals ps.ProductId into psGroup
                          from ps in psGroup.DefaultIfEmpty()
                          group new { p, ps } by p.Id into g
                          select new ProductStockViewModel
                          {
                              Product = g.FirstOrDefault()?.p,
                              Stock = g.Sum(item => item.ps?.Stock ?? 0)
                          }).ToList();
            switch (orderBy)
            {
                case "date_desc":
                    result = result.OrderByDescending(x => x.Product.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "price_desc":
                    result = result.OrderByDescending(x => x.Product.PromotionPrice ?? x.Product.Price).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "price":
                    result = result.OrderBy(x => x.Product.PromotionPrice ?? x.Product.Price).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                default:
                    result = result.OrderBy(x => x.Product.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
            }

            ViewBag.OrderBy = orderBy;
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

        public ActionResult ProductByCategory(long id, int page = 1, int pageSize = 8, string orderBy = "")
        {
            var data = _productCategoryService.GetProductCategoryById(id);
            int totalRecord = 0;

            int maxPage = 5;
            int totalPage = 0;

            if (data.ParentId == null)
            {
                var result = _productService.GetAllProductByRootCategory(data.Id, ref totalRecord, page, pageSize);
                switch (orderBy)
                {
                    case "date_desc":
                        result = result.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        break;
                    case "price_desc":
                        result = result.OrderByDescending(x => x.PromotionPrice ?? x.Price).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        break;
                    case "price":
                        result = result.OrderBy(x => x.PromotionPrice ?? x.Price).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        break;
                    default:
                        result = result.OrderBy(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        break;
                }

                ViewBag.OrderBy = orderBy;
                ViewBag.Category = data;
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
            var model = _productService.GetProductByCategoryId(id, ref totalRecord, page, pageSize);
            switch (orderBy)
            {
                case "date_desc":
                    model = model.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "price_desc":
                    model = model.OrderByDescending(x => x.PromotionPrice ?? x.Price).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "price":
                    model = model.OrderBy(x => x.PromotionPrice ?? x.Price).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                default:
                    model = model.OrderBy(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
            }

            ViewBag.OrderBy = orderBy;
            ViewBag.Category = data;
            ViewBag.TotalRecord = totalRecord;
            ViewBag.Page = page;

            totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        public ActionResult SaleProducts(int page = 1, int pageSize = 8)
        {
            var data = _productService.ListSaleProducts();
            int totalRecord = data.Count();
            var result = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            int maxPage = 5;
            int totalPage = 0;

            ViewBag.Category = data;
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

        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = _productCategoryService.GetAll();
            return PartialView(model);
        }

        public ActionResult Detail(long id)
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var product = _productService.GetProductById(id);
            var size = _productSizeService.GetProductSizeByProductId(product.Id);
            ViewBag.Category = _productCategoryService.GetProductCategoryById(product.CategoryId.Value);
            ViewBag.GetAllCategory = _productCategoryService.GetAll();
            ViewBag.RelateProducts = _productService.ListRelateProduct(id);
            ViewBag.Sizes = _sizeService.GetAll();
            ViewBag.SizeStock = (from ps in _productSizeService.GetProductSizeByProductId(product.Id)
                                 join s in _sizeService.GetAll() on ps.SizeId equals s.Id
                                 select new ProductDetailModel
                                 {
                                     ProductId = ps.ProductId,
                                     SizeId = ps.SizeId,
                                     SizeName = s.Name,
                                     Stock = ps.Stock
                                 }).ToList();
            ViewBag.Galleries = (from p in _productService.GetAll().Where(p => p.Id == id)
                                join g in _galleryService.GetAll() on p.GalleryId equals g.Id
                                select new ProductGalleryModel
                                {
                                    Product = p,
                                    Gallery = g
                                }).FirstOrDefault();
            ViewBag.Review = from r in _rateService.GetAll()
                             join u in _userService.GetAll() on r.UserId equals u.Id
                             where (r.ProductId == id && r.UserId == u.Id)
                             select new UserRateModel
                             {
                                 Rate = r,
                                 User = u
                             };
            if (user != null)
            {
                var data = from o in _orderService.GetAll()
                           join od in _orderDetailService.GetAll() on o.Id equals od.OrderId
                           join u in _userService.GetAll() on o.CustomerId equals u.Id
                           where (od.ProductId == id && o.CustomerId == user.UserId)
                           select new CustomerPerchasedModel
                           {
                               Account = u,
                               Order = o,
                               OrderDetail = od
                           };
                ViewBag.CustomerPurchased = data.Count();
            }

            Session["ProductId"] = product.Id;
            ViewBag.CountRate = CountRate(id);
            ViewBag.CountRateFiveStar = CountRateFiveStar(id);
            if (ViewBag.CountRate == 0)
            {
                ViewBag.AverageRate = null;
            }
            else
            {
                var rate = _rateService.GetRateByProductId(id);
                var rateSum = rate != null ? rate.Sum(x => x.Star) : 0;
                var countRate = rate.Count();
                var avgRate = (float)rateSum / countRate;

                ViewBag.AverageRate = avgRate;
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult ProductReviews(Rate review, string content)
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            review.CreatedDate = DateTime.Now;
            review.Content = content;
            review.Star = 0;
            review.UserId = _userService.GetUserByEmail(user.Email).Id;
            review.ProductId = (long)Session["ProductId"];

            _rateService.Insert(review);
            return RedirectToAction("Detail", "Product", new { id = review.ProductId });
        }

        public ActionResult ModalDetail(long id)
        {
            var product = _productService.GetProductById(id);
            var size = _productSizeService.GetProductSizeByProductId(product.Id);
            ViewBag.Category = _productCategoryService.GetProductCategoryById(product.CategoryId.Value);
            ViewBag.GetAllCategory = _productCategoryService.GetAll();
            ViewBag.Sizes = _sizeService.GetAll();
            ViewBag.SizeStock = (from ps in _productSizeService.GetProductSizeByProductId(product.Id)
                                 join s in _sizeService.GetAll() on ps.SizeId equals s.Id
                                 select new ProductDetailModel
                                 {
                                     ProductId = ps.ProductId,
                                     SizeId = ps.SizeId,
                                     SizeName = s.Name,
                                     Stock = ps.Stock
                                 }).ToList();
            ViewBag.Galleries = (from p in _productService.GetAll().Where(p => p.Id == id)
                                 join g in _galleryService.GetAll() on p.GalleryId equals g.Id
                                 select new ProductGalleryModel
                                 {
                                     Product = p,
                                     Gallery = g
                                 }).FirstOrDefault();
            return PartialView("~/Views/Product/ModalDetail.cshtml", product);
        }

        public int CountRateFiveStar(long id)
        {
            List<Product> product = _productService.GetAll();
            List<Rate> rate = _rateService.GetAll();
            int countRate = (from r in rate
                             join p in product
                             on r.ProductId equals p.Id
                             where (r.ProductId == id && r.ProductId == p.Id && r.Star == 5)
                             select new ProductRateModel
                             {
                                 Rate = r,
                                 Product = p
                             }).Count();
            return countRate;
        }

        public int CountRate(long id)
        {
            List<Product> product = _productService.GetAll();
            List<Rate> rate = _rateService.GetAll();
            int countRate = (from r in rate
                             join p in product
                             on r.ProductId equals p.Id
                             where (r.ProductId == id && r.ProductId == p.Id && r.Star > 0)
                             select new ProductRateModel
                             {
                                 Rate = r,
                                 Product = p
                             }).Count();
            return countRate;
        }

        public JsonResult ListName(string term)
        {
        var data = _productService.ListName(term);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Filter(string searchkeyword, decimal? minPrice, decimal? maxPrice, int page = 1, int pageSize = 8, string orderBy = "")
        {
            int totalRecord = 0;
            int maxPage = 5;
            int totalPage = 0;

            var result = (from p in _productService.GetAll().Where(x => x.Status == true)
                          join ps in _productSizeService.GetAll() on p.Id equals ps.ProductId into psGroup
                          from ps in psGroup.DefaultIfEmpty()
                          group new { p, ps } by p.Id into g
                          select new ProductStockViewModel
                          {
                              Product = g.FirstOrDefault()?.p,
                              Stock = g.Sum(item => item.ps?.Stock ?? 0)
                          }).ToList();

            totalRecord = result.Count();

            if (!string.IsNullOrEmpty(searchkeyword))
            {
                result = result.Where(x => x.Product.Name.Contains(searchkeyword)).ToList();
                totalRecord = result.Count();
            }

            if (minPrice.HasValue)
            {
                result = result.Where(item => item.Product.PromotionPrice.HasValue 
                                ? item.Product.PromotionPrice >= minPrice.Value
                                : item.Product.Price >= minPrice.Value).ToList();
                totalRecord = result.Count();
            }

            if (maxPrice.HasValue)
            {
                result = result.Where(item => item.Product.PromotionPrice.HasValue
                                ? item.Product.PromotionPrice <= maxPrice.Value
                                : item.Product.Price <= maxPrice.Value).ToList();
                totalRecord = result.Count();
            }

            switch (orderBy)
            {
                case "date_desc":
                    result = result.OrderByDescending(x => x.Product.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "price_desc":
                    result = result.OrderByDescending(x => x.Product.PromotionPrice ?? x.Product.Price).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "price":
                    result = result.OrderBy(x => x.Product.PromotionPrice ?? x.Product.Price).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                default:
                    result = result.OrderBy(x => x.Product.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
            }

            ViewBag.Search = searchkeyword;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.OrderBy = orderBy;

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
    }
}
