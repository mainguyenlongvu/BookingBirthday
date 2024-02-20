using Common;
using Data.Services;
using System;
using System.Linq;
using System.Web.Mvc;
using TGClothes.Models;

namespace TGClothes.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _userService;
        private readonly IProductService _productService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductStockService _productSizeService;

        public OrderController(IOrderService orderService, 
            IAccountService userService,
            IProductService productService, 
            IOrderDetailService orderDetailService,
            IProductStockService productSizeService)
        {
            _orderService = orderService;
            _userService = userService;
            _productService = productService;
            _orderDetailService = orderDetailService;
            _productSizeService = productSizeService;
        }
        // GET: Admin/Order

        public ActionResult Index(DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 8)
        {
            if (!fromDate.HasValue && !toDate.HasValue)
            {
                var model = _orderService.GetAllPaging(page, pageSize);
                return View(model);
            }
            else
            {
                var model = _orderService.GetAllByDatePaging(fromDate.Value, toDate.Value, page, pageSize);
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                return View(model);
            }
        }

        public ActionResult Details(long id)
        {
            var data = _orderDetailService.GetOrderDetailByOrderId(id);
            var customer = from o in _orderService.GetAll()
                           join u in _userService.GetAll() on o.CustomerId equals u.Id
                           where o.Id == id
                           select new OrderCustomerModel
                           {
                               Order = o,
                               Account = u
                           };

            var detail = from p in _productService.GetAll()
                         join od in _orderDetailService.GetAll() on p.Id equals od.ProductId
                         where od.OrderId == id && p.Id == od.ProductId
                         select new OrderDetailModel
                         {
                             Product = p,
                             OrderDetail = od
                         };

            ViewBag.Customer = customer;
            ViewBag.OrderDetail = detail;
            return View(data);
        }

        public ActionResult Edit(long id)
        {
            var model = _orderService.GetOrderById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(long id, OrderStatus status, DateTime date)
        {
            var order = _orderService.GetOrderById(id);
            var orderStatus = status;
            order.Id = id;
            if (!Enum.IsDefined(typeof(OrderStatus), status))
            {
                TempData["Errnull"] = "Vui lòng chọn tình trạng đơn hàng!";
            }
            else
            {
                order.Status = (int)orderStatus;
                order.DeliveryDate = date;
                _orderService.Update(order);

                if (order.Status == (int)OrderStatus.CANCELLED)
                {
                    var orderDetails = _orderDetailService.GetOrderDetailByOrderId(order.Id);

                    foreach (var orderDetail in orderDetails)
                    {
                        var stock = _productSizeService.GetProductSizeByProductIdAndSizeId(orderDetail.ProductId, orderDetail.SizeId);

                        // Cộng lại số lượng tồn của sản phẩm
                        stock.Stock += orderDetail.Quantity;
                        _productSizeService.Update(stock);
                    }
                }
                return RedirectToAction("Index");
            }
            return this.Edit(id);

        }
    }
}