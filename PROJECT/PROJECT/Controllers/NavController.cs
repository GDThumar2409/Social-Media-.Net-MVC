using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PROJECT.Controllers
{
    public class NavController : Controller
    {
        // GET: Nav
        public ActionResult Aboutus()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Jobs()
        {
            return View();
        }
    }
}