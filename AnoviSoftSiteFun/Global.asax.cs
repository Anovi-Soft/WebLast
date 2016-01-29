using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AnoviSoftSiteFun.Helpers;

namespace AnoviSoftSiteFun
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static VisitorCounter counter = VisitorCounter.Instance;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        public void Session_OnStart()
        {
            counter.OpenSession(Request, User);
        }

        protected void Application_PreSendRequestContent(object sender, EventArgs e)
        {
            if (Request.Url.AbsolutePath.StartsWith("/__browserLink")) return;
            var request = Request;
            var user = User;
            counter.PageVisit(Request, user);
        }


    }

    
}
