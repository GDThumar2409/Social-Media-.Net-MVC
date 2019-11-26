using PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PROJECT.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        // GET: Notification
        ApplicationDbContext ApplicationDbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            ApplicationUser applicationUser = Session["User"] as ApplicationUser;
            ViewBag.Notificatons = applicationUser.Notifications;
            return View();
        }
    }
}