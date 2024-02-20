using Common;
using Common.Payment;
using Data.EF;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TGClothes.Models;

namespace TGClothes.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISizeService _sizeService;
        private readonly IProductStockService _productSizeService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;

        public CartController(
            IProductService productService, 
            ISizeService sizeService, 
            IProductStockService productSizeService, 
            IOrderDetailService orderDetailService, 
            IOrderService orderService, IAccountService accountService)
        {
            _productService = productService;
            _sizeService = sizeService;
            _productSizeService = productSizeService;
            _orderDetailService = orderDetailService;
            _orderService = orderService;
            _accountService = accountService;
        }

        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CommonConstants.CART_SESSION];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            ViewBag.SubTotal = SubTotal();
            ViewBag.Promotion = Promotion();
            return View(list);
        }

        public ActionResult AddToCart(CartModel model)
        {
            var product = _productService.GetProductById(model.ProductId);
            var size = _sizeService.GetSizeById(model.SizeId);
            var cart = Session[CommonConstants.CART_SESSION];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.Id == model.ProductId && x.Size.Id == model.SizeId))
                {
                    foreach (var item in list)
                    {
                        var stock = _productSizeService.GetStock(item.Product.Id, item.Size.Id);
                        if (item.Product.Id == model.ProductId && item.Size.Id == model.SizeId)
                        {
                            if (item.Quantity < stock && stock > 0)
                            {
                                item.Quantity += model.Quantity;
                                return Json(new { success = true, message = "Thêm vào giỏ hàng thành công." }, JsonRequestBehavior.AllowGet);
                            }

                            else
                            {
                                return Json(new { success = false, message = "Sản phẩm không được vượt quá số lượng tồn." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
                else
                {
                    //tao moi doi tuong cart item
                    var item = new CartItem();
                    item.Product = product;
                    item.Size = size;
                    item.Quantity = model.Quantity;
                    list.Add(item);
                    return Json(new { success = true, message = "Thêm vào giỏ hàng thành công." }, JsonRequestBehavior.AllowGet);
                }
                //gan vao session
                Session[CommonConstants.CART_SESSION] = list;

            }
            else
            {
                //tao moi doi tuong cart item
                var item = new CartItem();
                item.Product = product;
                item.Size = size;
                item.Quantity = model.Quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //gan vao session
                Session[CommonConstants.CART_SESSION] = list;

            }
            return Json(new { success = true, message = "Thêm vào giỏ hàng thành công." }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CommonConstants.CART_SESSION];
            
            foreach (var item in sessionCart)
            {
                var stock = _productSizeService.GetStock(item.Product.Id, item.Size.Id);
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.Id == item.Product.Id && x.Size.Id == item.Size.Id);
                if (jsonItem != null && jsonItem.Quantity <= stock)
                {
                    item.Quantity = jsonItem.Quantity;
                }
                else
                {
                    TempData["msg"] = "<script>alert('Sản phẩm không được vượt quá số lượng tồn');</script>";
                    break;
                }
            }
            Session[CommonConstants.CART_SESSION] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(long productId, long sizeId)
        {
            var sessionCart = (List<CartItem>)Session[CommonConstants.CART_SESSION];
            sessionCart.RemoveAll(x => x.Product.Id == productId && x.Size.Id == sizeId);
            Session[CommonConstants.CART_SESSION] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult DeleteAll()
        {
            Session[CommonConstants.CART_SESSION] = null;
            return Json(new
            {
                status = true
            });
        }

        public ActionResult Payment()
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (Session[CommonConstants.USER_SESSION] == null || Session[CommonConstants.USER_SESSION].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            var customer = _accountService.GetUserById(user.UserId);
            var data = new CustomerInfo()
            {
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address
            };

            var cart = Session[CommonConstants.CART_SESSION];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            ViewBag.SubTotal = SubTotal();
            ViewBag.Promotion = Promotion();
            return View(data);
        }

        #region COD
        [HttpPost]
        public ActionResult PaymentCOD(CustomerInfo model)
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var order = new Order();
            order.OrderDate = DateTime.Now;
            order.CustomerId = user.UserId;
            order.Name = model.Name;
            order.Email = model.Email;
            order.Phone = model.Phone;
            order.DeliveryAddress = model.Address;
            order.Status = (int)OrderStatus.PENDING;
            order.PaymentMethod = (int)PaymentMethods.COD;

            var id = _orderService.Insert(order);
            var cart = (List<CartItem>)Session[CommonConstants.CART_SESSION];
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail();
                orderDetail.ProductId = item.Product.Id;
                orderDetail.OrderId = id;
                orderDetail.SizeId = item.Size.Id;
                orderDetail.Price = item.Product.PromotionPrice ?? item.Product.Price;
                orderDetail.Quantity = item.Quantity;
                orderDetail.TotalQuantity = TotalQuantity();
                orderDetail.TotalPrice = Total();

                var stock = _productSizeService.GetProductSizeByProductIdAndSizeId(item.Product.Id, item.Size.Id);
                stock.Stock -= orderDetail.Quantity;
                _productSizeService.Update(stock);

                _orderDetailService.Insert(orderDetail);
            }
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/template/neworder.html"));
            content = content.Replace("{{CustomerName}}", model.Name);
            content = content.Replace("{{Phone}}", model.Phone);
            content = content.Replace("{{OrderDate}}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            content = content.Replace("{{Address}}", model.Address);
            content = content.Replace("{{Total}}", Total().ToString("N0"));

            new MailHelper().SendMail(model.Email, "Đơn hàng mới từ TunaClothes", content);
            Session[CommonConstants.CART_SESSION] = null;
            return Redirect("/hoan-thanh");
        }
        #endregion

        #region VN Pay
        public ActionResult PaymentVnPay()
        {
            var name = Request.Form["name"]; 
            var email = Request.Form["email"]; 
            var phone = Request.Form["address"];
            var address = Request.Form["address"];

            //Thanh toán thành công
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var order = new Order();
            order.OrderDate = DateTime.Now;
            order.CustomerId = user.UserId;
            order.Name = name;
            order.Email = email;
            order.Phone = phone;
            order.DeliveryAddress = address;
            order.Status = (int)OrderStatus.PENDING;
            order.PaymentMethod = (int)PaymentMethods.VNPAY;

            var id = _orderService.Insert(order);
            var cart = (List<CartItem>)Session[CommonConstants.CART_SESSION];
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail();
                orderDetail.ProductId = item.Product.Id;
                orderDetail.OrderId = id;
                orderDetail.SizeId = item.Size.Id;
                orderDetail.Price = item.Product.PromotionPrice ?? item.Product.Price;
                orderDetail.Quantity = item.Quantity;
                orderDetail.TotalQuantity = TotalQuantity();
                orderDetail.TotalPrice = Total();

                var stock = _productSizeService.GetProductSizeByProductIdAndSizeId(item.Product.Id, item.Size.Id);
                stock.Stock -= orderDetail.Quantity;
                _productSizeService.Update(stock);

                _orderDetailService.Insert(orderDetail);
            }

            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];
            string amount = (Total() * 100).ToString();
            VnPayLibrary pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", VnPayLibrary.VERSION); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", amount); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", "VNBANK"); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", order.Id.ToString()); //mã hóa đơn
            
            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }

        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                VnPayLibrary pay = new VnPayLibrary();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_") || s.StartsWith("vnp_Inv_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);

                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        Session[CommonConstants.CART_SESSION] = null;
                        return Redirect("/hoan-thanh");
                    }
                    else
                    {
                        var order = _orderService.GetOrderById(orderId);
                        var orderDetail = _orderDetailService.GetOrderDetailByOrderId(order.Id);
                        foreach (var item in orderDetail)
                        {
                            _orderDetailService.Delete(item);
                        }
                        _orderService.Delete(order.Id);
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        TempData["PaymentFailure"] = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                        return RedirectToAction("OrderFailure");
                    }
                }
                else
                {
                    TempData["PaymentFailure"] = "Có lỗi xảy ra trong quá trình xử lý";
                    return Redirect("/that-bai");
                }
            }

            return View();
        }
        #endregion

        public ActionResult OrderSuccess()
        {
            return View();
        }

        public ActionResult OrderFailure()
        {
            ViewBag.Message = TempData["PaymentFailure"];
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult CartModel()
        {
            var cart = Session[CommonConstants.CART_SESSION];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }

        private decimal SubTotal()
        {
            decimal subtotal = 0;
            List<CartItem> cart = Session[CommonConstants.CART_SESSION] as List<CartItem>;
            if (cart != null)
            {
                subtotal = cart.Sum(n => n.Quantity * n.Product.Price.Value);
            }
            return subtotal;
        }

        private decimal Promotion()
        {
            decimal promotion = 0;
            List<CartItem> cart = Session[CommonConstants.CART_SESSION] as List<CartItem>;
            if (cart != null)
            {
                promotion = cart.Sum(n => n.Quantity * n.Product.Price.Value * (n.Product.Promotion.HasValue ? n.Product.Promotion.Value : 0) / 100);
            }
            return promotion;
        }

        private int TotalQuantity()
        {
            int total = 0;
            List<CartItem> cart = Session[CommonConstants.CART_SESSION] as List<CartItem>;
            if (cart != null)
            {
                total = cart.Sum(n => n.Quantity);
            }
            return total;
        }

        private decimal Total()
        {
            decimal total = 0;
            List<CartItem> cart = Session[CommonConstants.CART_SESSION] as List<CartItem>;
            if (cart != null)
            {
                foreach (var item in cart)
                {
                    decimal itemPrice = item.Product.Price.Value;

                    if (item.Product.PromotionPrice != null)
                    {
                        itemPrice = item.Product.PromotionPrice.Value;
                    }

                    total += item.Quantity * itemPrice;
                }
            }
            return total;
        }
    }
}