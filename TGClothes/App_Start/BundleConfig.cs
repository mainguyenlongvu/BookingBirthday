using System.Web;
using System.Web.Optimization;

namespace TGClothes
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/adminjs").Include(
                        "~/Assets/Admin/vendor/jquery/jquery.min.js",
                        "~/Assets/Admin/vendor/jquery/jquery-3.6.0.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Assets/Admin/vendor/bootstrap/js/bootstrap.bundle.min.js",
                        "~/Assets/Admin/vendor/jquery-easing/jquery.easing.min.js",
                        "~/Assets/Admin/plugin/ckfinder/ckfinder.js",
                        "~/Assets/Admin/plugin/ckeditor/ckeditor.js",
                        "~/Assets/Admin/js/sb-admin-2.js",
                        "~/Assets/Admin/js/sb-admin-2.min.js"
                        ));

            bundles.Add(new StyleBundle("~/bundles/admincss").Include(
                      "~/Assets/Admin/vendor/fontawesome-free/css/all.min.css",
                      "~/Assets/Admin/css/sb-admin-2.min.css",
                      "~/Content/PagedList.css"
                      ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
