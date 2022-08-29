using System.Web;
using System.Web.Optimization;

namespace ioxdemo
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.3.1.js"
                        ));
            

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap.bundle.min.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/DateTimePicker.min.js",
                      "~/Scripts/jquery.toast.min.js",
                      "~/Scripts/jquery.countdown.min.js",
                      "~/Scripts/jquery.validate*"
                      ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-grid.css",
                      "~/Content/font-awesome.css",
                      "~/Content/DTPickerPetite.css",
                      "~/Content/jQuery.countdownTimer.css",
                      "~/Content/jquery.toast.min.css",
                      "~/Content/site.css"));
        }

    }
}
