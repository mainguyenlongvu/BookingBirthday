using Data.EF;
using Data.Services;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using TGClothes.Models;

namespace TGClothes.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IRateService _rateService;
        private readonly IAccountService _userService;
        private readonly IGalleryService _galleryService;
        private readonly IProductStockService _productSizeService;
        private readonly ISizeService _sizeService;

        public ProductController(
            IProductService productService, 
            IProductCategoryService productCategoryService, 
            IRateService rateService, 
            IAccountService userService,
            IGalleryService galleryService,
            IProductStockService productSizeService,
            ISizeService sizeService
            )
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _rateService = rateService;
            _userService = userService;
            _galleryService = galleryService;
            _productSizeService = productSizeService;
            _sizeService = sizeService;
        }

        // GET: Admin/Product
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            if (searchString != null)
            {
                var products = (from p in _productService.GetAll()
                                join ps in _productSizeService.GetAll() on p.Id equals ps.ProductId into psGroup
                                from ps in psGroup.DefaultIfEmpty()
                                where p.Name.Contains(searchString)
                                group new { p, ps } by p.Id into g
                                select new ProductStockViewModel
                                {
                                    Product = g.FirstOrDefault()?.p,
                                    Stock = g.Sum(item => item.ps?.Stock ?? 0)
                                }).ToList();
                ViewBag.Search = searchString;
                return View(products.ToPagedList(page, pageSize));
            }
            else
            {
                var products = (from p in _productService.GetAll()
                                join ps in _productSizeService.GetAll() on p.Id equals ps.ProductId into psGroup
                                from ps in psGroup.DefaultIfEmpty()
                                group new { p, ps } by p.Id into g
                                select new ProductStockViewModel
                                {
                                    Product = g.FirstOrDefault()?.p,
                                    Stock = g.Sum(item => item.ps?.Stock ?? 0)
                                }).ToList();
                //var all_sach = (from s in _context.Products select s).OrderBy(m => m.ProductId);
                return View(products.ToPagedList(page, pageSize));
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new ProductModel();
            model.Sizes = _sizeService.GetAll();
            SetViewBag();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ProductModel model, long? ParentId)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Id = model.Product.Id,
                    Name = model.Product.Name,
                    MetaTitle = model.Product.MetaTitle,
                    Description = model.Product.Description,
                    Details = model.Product.Details,
                    Image = model.Product.Image,
                    Price = model.Product.Price,
                    OriginalPrice = model.Product.OriginalPrice,
                    Promotion = model.Product.Promotion,
                    PromotionPrice = model.Product.Price - (model.Product.Price * model.Product.Promotion / 100),
                    CategoryId = ParentId,
                    CreatedDate = DateTime.Now,
                    Status = true,
                };

                long id = _productService.Insert(product);
                if (id > 0)
                {
                    List<ProductSize> productSizeList = model.ProductSizes;
                    foreach (var sizeStock in productSizeList)
                    {
                        long sizeId = sizeStock.SizeId;
                        int stock = sizeStock.Stock;

                        var productSize = new ProductSize
                        {
                            ProductId = id,
                            SizeId = sizeId,
                            Stock = stock
                        };

                        _productSizeService.Insert(productSize);
                    }

                    var gallery = new Gallery
                    {
                        Image1 = model.Gallery.Image1,
                        Image2 = model.Gallery.Image2,
                        Image3 = model.Gallery.Image3
                    };

                    long galleryId = _galleryService.Insert(gallery);
                    if (galleryId > 0)
                    {
                        product.GalleryId = galleryId;
                        _productService.Update(product);
                    }

                    SetAlert("Thêm mới sản phẩm thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm mới sản phẩm không thành công.");
                }
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var product = _productService.GetProductById(id);
            var model = new ProductModel();
            model.Product = product;
            model.ProductSizes = _productSizeService.GetProductSizeByProductId(product.Id);
            model.Gallery = _galleryService.GetGalleryById(product.GalleryId.Value);
            model.Sizes = _sizeService.GetAll();
            SetViewBag(model.Product.CategoryId);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _productService.GetProductById(model.Product.Id);
                if (product != null)
                {
                    product.Id = model.Product.Id;
                    product.Name = model.Product.Name;
                    product.MetaTitle = model.Product.MetaTitle;
                    product.Description = model.Product.Description;
                    product.Details = model.Product.Details;
                    product.Image = model.Product.Image;
                    product.Price = model.Product.Price;
                    product.OriginalPrice = model.Product.OriginalPrice;
                    product.Promotion = model.Product.Promotion;
                    product.PromotionPrice = model.Product.Price - (model.Product.Price * model.Product.Promotion / 100);
                    product.CategoryId = model.Product.CategoryId;
                    product.ModifiedDate = DateTime.Now;
                    product.Status = true;
                }

                var id = _productService.Update(product);
                if (id)
                {
                    List<ProductSize> productSizeList = model.ProductSizes;
                    foreach (var sizeStock in productSizeList)
                    {
                        long sizeId = sizeStock.SizeId;
                        int stock = sizeStock.Stock;

                        // Kiểm tra xem size đã tồn tại cho sản phẩm hay chưa
                        var existingProductSize = _productSizeService.GetProductSizeByProductIdAndSizeId(model.Product.Id, sizeId);

                        if (existingProductSize != null)
                        {
                            //foreach(var item in existingProductSize)
                            //{
                                existingProductSize.Stock = stock;
                                _productSizeService.Update(existingProductSize);
                            //}
                            // Nếu đã tồn tại, cập nhật số lượng trong kho
                        }
                        else
                        {
                            // Nếu chưa tồn tại, tạo mới bản ghi ProductSize
                            var newProductSize = new ProductSize
                            {
                                ProductId = model.Product.Id,
                                SizeId = sizeId,
                                Stock = stock
                            };

                            _productSizeService.Insert(newProductSize);
                        }
                    }

                    var gallery = _galleryService.GetGalleryById(product.GalleryId.Value);

                    if (gallery != null)
                    {
                        // Cập nhật thông tin của gallery
                        gallery.Image1 = model.Gallery.Image1;
                        gallery.Image2 = model.Gallery.Image2;
                        gallery.Image3 = model.Gallery.Image3;

                        _galleryService.Update(gallery);
                    }
                    else
                    {
                        // Tạo mới gallery nếu chưa tồn tại
                        gallery = new Gallery
                        {
                            Image1 = model.Gallery.Image1,
                            Image2 = model.Gallery.Image2,
                            Image3 = model.Gallery.Image3
                        };

                        long galleryId = _galleryService.Insert(gallery);

                        if (galleryId > 0)
                        {
                            product.GalleryId = galleryId;
                            _productService.Update(product);
                        }
                    }
                    SetAlert("Cập nhật sản phẩm thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật sản phẩm không thành công.");
                }
            }
            SetViewBag(model.Product.CategoryId);
            return View("Index");
        }

        public ActionResult Detail(long id)
        {
            var product = _productService.GetProductById(id);
            var productSizes = (from p in _productSizeService.GetProductSizeByProductId(id)
                                join s in _sizeService.GetAll() on p.SizeId equals s.Id
                                select new SizeModel
                                {
                                    SizeId = p.SizeId,
                                    SizeName = s.Name,
                                    Stock = p.Stock
                                }).ToList();
            var gallery = _galleryService.GetGalleryById(product.GalleryId.Value);
            ViewBag.Product = product;
            ViewBag.ProductSizes = productSizes;
            ViewBag.ProductGallery = gallery;
            return View();
        }

        public JsonResult Delete(long id)
        {
            var check = _productService.Delete(id);
            if (check)
            {
                return Json(new { status = true, message = "Xóa thành công" });
            }
            else
            {
                return Json(new { status = false, message = "Sản phẩm đã được bán, không thể xóa khỏi CSDL" });
            }
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = _productService.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public void SetViewBag(long? selectedId = null)
        {
            var productCategories = _productCategoryService.GetAll();
            var sizes = _sizeService.GetAll();
            ViewBag.CategoryId = new SelectList(productCategories, "Id", "Name", selectedId);
            ViewBag.Sizes = sizes;
        }
    }
}