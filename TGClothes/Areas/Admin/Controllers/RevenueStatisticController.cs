using Data.Services;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using TGClothes.Models;

namespace TGClothes.Areas.Admin.Controllers
{
    public class RevenueStatisticController : BaseController
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public RevenueStatisticController(IOrderDetailService orderDetailService, IOrderService orderService, IProductService productService)
        {
            _orderDetailService = orderDetailService;
            _orderService = orderService;
            _productService = productService;
        }
        // GET: Admin/RevenueStatistic
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderStatistic(DateTime? fromDate, DateTime? toDate)
        {
            if (!fromDate.HasValue && !toDate.HasValue)
            {

                var data = (from o in _orderService.GetAll()
                            join od in _orderDetailService.GetAll() on o.Id equals od.OrderId
                            where o.OrderDate.Month == DateTime.Now.Month && o.Status == (int)OrderStatus.SUCCESSFUL
                            group o by o.OrderDate.Date into g
                            select new
                            {
                                Date = g.Key,
                                TotalOrdersSold = g.Count()
                            }).ToList();

                ViewBag.TotalOrder = data.Sum(x => x.TotalOrdersSold);
                ViewBag.NumOfOrder = data.Select(x => x.TotalOrdersSold);
                ViewBag.DateOrder = data.Select(x => x.Date.ToString("yyyy-MM-dd")).ToList();
                return View(data);
            }
            else
            {
                var data = (from o in _orderService.GetAll()
                            join od in _orderDetailService.GetAll() on o.Id equals od.OrderId
                            where o.OrderDate >= fromDate && o.OrderDate <= toDate && o.Status == (int)OrderStatus.SUCCESSFUL
                            group o by o.OrderDate.Date into g
                            select new
                            {
                                Date = g.Key,
                                TotalOrdersSold = g.Count()
                            }).ToList();

                ViewBag.TotalOrder = data.Sum(x => x.TotalOrdersSold);
                ViewBag.NumOfOrder = data.Select(x => x.TotalOrdersSold);
                ViewBag.DateOrder = data.Select(x => x.Date.ToString("yyyy-MM-dd")).ToList();
                return View(data);
            }
        }

        public ActionResult RevenueStatistic(DateTime? fromDate, DateTime? toDate)
        {
            if (!fromDate.HasValue && !toDate.HasValue)
            {

                var data = (from o in _orderService.GetAll()
                            join od in _orderDetailService.GetAll() on o.Id equals od.OrderId
                            join p in _productService.GetAll() on od.ProductId equals p.Id
                            where o.OrderDate.Month == DateTime.Now.Month && o.Status == (int)OrderStatus.SUCCESSFUL
                            group new { od, p } by o.OrderDate.Date into g
                            select new
                            {
                                Date = g.Key,
                                Revenue = g.Sum(x => x.od.Price * x.od.Quantity),
                                Profit = g.Sum(x => (x.od.Price - x.p.OriginalPrice) * x.od.Quantity)
                            }).ToList();

                ViewBag.TotalRevenue = data.Sum(x => x.Revenue);
                ViewBag.TotalProfit = data.Sum(x => x.Profit);
                ViewBag.RevenueOfDay = data.Select(x => x.Revenue);
                ViewBag.ProfitOfDay = data.Select(x => x.Profit);
                ViewBag.DateOrder = data.Select(x => x.Date.ToString("yyyy-MM-dd")).ToList();
                return View(data);
            }
            else
            {
                var data = (from o in _orderService.GetAll()
                            join od in _orderDetailService.GetAll() on o.Id equals od.OrderId
                            join p in _productService.GetAll() on od.ProductId equals p.Id
                            where o.OrderDate >= fromDate && o.OrderDate <= toDate && o.Status == (int)OrderStatus.SUCCESSFUL
                            group new { od, p } by o.OrderDate.Date into g
                            select new
                            {
                                Date = g.Key,
                                Revenue = g.Sum(x => x.od.Price * x.od.Quantity),
                                Profit = g.Sum(x => (x.od.Price - x.p.OriginalPrice) * x.od.Quantity)
                            }).ToList();

                ViewBag.TotalRevenue = data.Sum(x => x.Revenue);
                ViewBag.TotalProfit = data.Sum(x => x.Profit);
                ViewBag.RevenueOfDay = data.Select(x => x.Revenue);
                ViewBag.ProfitOfDay = data.Select(x => x.Profit);
                ViewBag.DateOrder = data.Select(x => x.Date.ToString("yyyy-MM-dd")).ToList();
                return View(data);
            }
        }

        public ActionResult ProductStatistic(DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 10)
        {
            if (!fromDate.HasValue && !toDate.HasValue)
            {
                var result = (from o in _orderService.GetAll()
                            join od in _orderDetailService.GetAll() on o.Id equals od.OrderId
                            join p in _productService.GetAll() on od.ProductId equals p.Id
                            where o.OrderDate.Month == DateTime.Now.Month && o.Status == (int)OrderStatus.SUCCESSFUL
                            group new { od, p } by od.ProductId into g
                            select new ProductStatistic()
                            {
                                ProductSold = g.Sum(x => x.od.Quantity),
                                Product = _productService.GetProductById(g.Key),
                                Price = g.First().od.Price.Value
                            });

                var totalCount = result.ToList();
                var data = result.OrderByDescending(x => x.ProductSold).ToPagedList(page, pageSize);

                ViewBag.TotalProductSold = totalCount.Sum(x => x.ProductSold);
                ViewBag.ProductSoldOfDay = data.Select(x => x.ProductSold);
                return View(data);
            }
            else
            {
                var result = (from o in _orderService.GetAll()
                            join od in _orderDetailService.GetAll() on o.Id equals od.OrderId
                            join p in _productService.GetAll() on od.ProductId equals p.Id
                            where o.OrderDate >= fromDate && o.OrderDate <= toDate && o.Status == (int)OrderStatus.SUCCESSFUL
                            group new { od, p } by od.ProductId into g
                            select new ProductStatistic()
                            {
                                ProductSold = g.Sum(x => x.od.Quantity),
                                Product = _productService.GetProductById(g.Key),
                                Price = g.First().od.Price.Value
                            });

                var totalCount = result.ToList();
                var data = result.OrderByDescending(x => x.ProductSold).ToPagedList(page, pageSize);

                ViewBag.TotalProductSold = totalCount.Sum(x => x.ProductSold);
                ViewBag.ProductSoldOfDay = data.Select(x => x.ProductSold);
                return View(data);
            }
        }
    }
}