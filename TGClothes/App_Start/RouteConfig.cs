using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TGClothes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
               name: "Product",
               url: "san-pham",
               defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "TGClothes.Controllers" }
           );

            routes.MapRoute(
              name: "Order",
              url: "don-hang",
              defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "TGClothes.Controllers" }
          );

            routes.MapRoute(
               name: "Filter Product",
               url: "san-pham/filter",
               defaults: new { controller = "Product", action = "Filter", id = UrlParameter.Optional },
               namespaces: new[] { "TGClothes.Controllers" }
           );

            routes.MapRoute(
               name: "Sale Product",
               url: "san-pham/sale",
               defaults: new { controller = "Product", action = "SaleProducts", id = UrlParameter.Optional },
               namespaces: new[] { "TGClothes.Controllers" }
           );

            routes.MapRoute(
                name: "About",
                url: "gioi-thieu",
                defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Content",
                url: "tin-tuc",
                defaults: new { controller = "Content", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Content Details",
                url: "tin-tuc/{metatitle}-{Id}",
                defaults: new { controller = "Content", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Tags",
                url: "tag/{tagId}",
                defaults: new { controller = "Content", action = "Tag", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Categories",
                url: "category/{categoryId}",
                defaults: new { controller = "Content", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            //routes.MapRoute(
            //    name: "Product Category",
            //    url: "san-pham/{metatitle}-{productCategoryId}",
            //    defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
            //    namespaces: new[] { "TGClothes.Controllers" }
            //);

            routes.MapRoute(
                name: "Product Category",
                url: "san-pham/{metatitle}-{id}",
                defaults: new { controller = "Product", action = "ProductByCategory", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Search Product",
                url: "tim-kiem",
                defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Product Details",
                url: "chi-tiet/{metatitle}-{Id}",
                defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
               name: "Order Details",
               url: "don-hang/chi-tiet-{Id}",
               defaults: new { controller = "Order", action = "Details", id = UrlParameter.Optional },
               namespaces: new[] { "TGClothes.Controllers" }
           );

            routes.MapRoute(
                name: "Cart",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Login with google",
                url: "signin-google",
                defaults: new { controller = "User", action = "LoginWithGoogle", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Forgot Password",
                url: "quen-mat-khau",
                defaults: new { controller = "User", action = "ForgotPassword", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Regisetr",
                url: "dang-ky",
                defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "dang-xuat",
                defaults: new { controller = "User", action = "Logout", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Add To Cart",
                url: "them-gio-hang",
                defaults: new { controller = "Cart", action = "AddToCart", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Payment",
                url: "thanh-toan",
                defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "VNPay Payment",
                url: "thanh-toan-vnpay",
                defaults: new { controller = "Cart", action = "PaymentVnPay", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "COD Payment",
                url: "thanh-toan-cod",
                defaults: new { controller = "Cart", action = "PaymentCOD", id = UrlParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Order Sussess",
                url: "hoan-thanh",
                defaults: new { controller = "Cart", action = "OrderSuccess", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Order Failure",
                url: "that-bai",
                defaults: new { controller = "Cart", action = "OrderFailure", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Controllers" }
            );
        }
    }
}
