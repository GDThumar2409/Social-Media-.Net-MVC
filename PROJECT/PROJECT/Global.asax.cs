using Microsoft.AspNet.SignalR;
using PROJECT.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace PROJECT
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(50);
            List<string> onlineUsers = new List<string>();
            Application["OnlineUsers"] = onlineUsers;
            
        }
    }
}
