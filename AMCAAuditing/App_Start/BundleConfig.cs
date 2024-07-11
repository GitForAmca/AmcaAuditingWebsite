using System.Web;
using System.Web.Optimization;

namespace AMCAAuditing
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

           

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/line-awesome.min.css",
                      "~/Content/owl.carousel.min.css",
                      "~/Content/owl.theme.default.min.css",
                      "~/Content/datepicker.css",
                      "~/Content/bootstrap-select.min.css",
                      "~/Content/main.css"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/custom.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/bootstrap-select.min.js",
                      "~/Scripts/owl.carousel.min.js"));

            // JQuery validator.   
            bundles.Add(new ScriptBundle("~/bundles/custom-validator").Include(
                                  "~/Scripts/script-custom-validator.js"));
        }
    }
}
