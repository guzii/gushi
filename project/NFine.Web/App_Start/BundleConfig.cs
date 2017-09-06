using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;


namespace NFine.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //主题
            bundles.Add(new StyleBundle("~/Content/metro/css").Include(
                       "~/Content/theme/build/css/metro.min.css",
                       "~/Content/theme/build/css/metro-responsive.min.css",
                       "~/Content/theme/build/css/metro-icons.min.css",
                       "~/Content/theme/build/css/metro-schemes.min.css"
                       ));

            bundles.Add(new ScriptBundle("~/Content/metro/js").Include(
                       "~/Content/js/jquery/jquery-2.1.1.min.js",
                       "~/Content/theme/build/js/metro.min.js"));
        }
    }
}