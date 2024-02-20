using Common;
using Data.EF;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGClothes.Models;

namespace TGClothes.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IAccountService _userService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public HomeController(
            IOrderDetailService orderDetailService, 
            IAccountService userService, 
            IOrderService orderService, 
            IProductService productService)
        {
            _orderDetailService = orderDetailService;
            _userService = userService;
            _orderService = orderService;
            _productService = productService;
        }
        // GET: Admin/Home
        public ActionResult Index()
        {
            ViewBag.MonthlyRevenue = MonthlyRevenue();
            ViewBag.AnnualRevenue = AnnualRevenue();
            ViewBag.TotalCustomer = CustomerStatistic();
            ViewBag.TotalOrder = OrderStatistic();
            ViewBag.TotalProduct = ProductStatistic();
            ViewBag.Top10Product = Top10Product();
            ViewBag.RateOfDelivery = RateOfDelivery();
            return View();
        }

        #region Doanh thu
        public decimal MonthlyRevenue()
        {
            return _orderDetailService.MonthlyRevenue();
        }

        public decimal AnnualRevenue()
        {
            return _orderDetailService.AnnualRevenue();
        }

        public double CustomerStatistic()
        {
            return _userService.CustomerStatistic();
        }

        public double OrderStatistic()
        {
            return _orderService.OrderStatistic();
        }

        public double ProductStatistic()
        {
            return _productService.ProductStatistic();
        }

        public List<Top10Product> Top10Product()
        {
            var data = (from p in _productService.GetAll()
                        join od in _orderDetailService.GetAll() on p.Id equals od.ProductId
                        join o in _orderService.GetAll() on od.OrderId equals o.Id
                        where o.OrderDate.Month == DateTime.Now.Month && o.OrderDate.Year == DateTime.Now.Year && o.Status == (int)OrderStatus.SUCCESSFUL
                        group new { od, p } by od.ProductId into g
                        let totalQuantitySold = g.Sum(x => x.od.Quantity)
                        orderby totalQuantitySold descending
                        select new Top10Product()
                        {
                            ProductId = g.Key,
                            ProductName = g.Select(x => x.p.Name).FirstOrDefault(),
                            TotalQuantitySold = totalQuantitySold,
                            TotalRevenue = g.Sum(x => x.od.Price.Value * x.od.Quantity)
                        }).Take(10).ToList();

            var listProduct = data.Select(x => x.ProductName);
            var revenueOfProduct = data.Select(x => x.TotalRevenue);
            ViewBag.NameOfProduct = listProduct;
            ViewBag.RevenueOfProduct = revenueOfProduct;
            return data;
        }

        public bool RateOfDelivery ()
        {
            var success = _orderService.GetAll().Where(x => x.Status == (int)OrderStatus.SUCCESSFUL).Count();
            var failure = _orderService.GetAll().Where(x => x.Status == (int)OrderStatus.CANCELLED).Count();
            var total = success + failure;
            float rateSuccess = (float)success / total * 100;
            float rateFailure = (float)failure / total * 100;
            ViewBag.RateSuccess = rateSuccess;
            ViewBag.RateFailure = rateFailure;
            return true;
        }
        #endregion
    }
}