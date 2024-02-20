using Data.Services;
using Data.Services.ServiceImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Unity.AspNet.Mvc;

namespace TGClothes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // T?o m?t Unity Container
            var container = new UnityContainer();

            // ??ng ký các ph? thu?c
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<INewsCategoryService, NewsCategoryService>();
            container.RegisterType<INewsService, NewsService>();
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IProductCategoryService, ProductCategoryService>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<ISizeService, SizeService>();
            container.RegisterType<ISlideService, SlideService>();
            container.RegisterType<IRateService, RateService>();
            container.RegisterType<IGalleryService, GalleryService>();
            container.RegisterType<IProductStockService, ProductStockService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IOrderDetailService, OrderDetailService>();
            container.RegisterType<IFeedbackService, FeedbackService>();

            // ??t Resolver m?c ??nh cho MVC DependencyResolver
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
