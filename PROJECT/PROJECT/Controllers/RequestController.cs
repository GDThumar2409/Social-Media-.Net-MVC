using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PROJECT.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        ApplicationDbContext ApplicationDb = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;

        public RequestController()
        {
        }

        public RequestController(ApplicationSignInManager signInManager)
        {
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // GET: Request
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(String UserName)
        {
            ApplicationUser applicationUser = Session["User"] as ApplicationUser;
            List<ApplicationUser> users = ApplicationDb.Users.Where(u => u.UserName == UserName).ToList<ApplicationUser>();
            
            ViewBag.Search = users;
            return View("Index");
        }

        public ActionResult File(string id)
        {
            ApplicationUser user = ApplicationDb.Users.Where(u => u.Id == id).First<ApplicationUser>();
            var fileToRetrieve= user.Dp;
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }

        public ActionResult Send(string UserId)
        {
            ApplicationUser applicationUser = Session["User"] as ApplicationUser;
            if(applicationUser.Id == UserId)
            {
                return Json("Can not send Request", JsonRequestBehavior.AllowGet);
            }
            Requests request = new Requests();
            var flag = ApplicationDb.Requests.Where(r => r.to_id == UserId && r.from_id == applicationUser.Id).Count();
            //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            //javaScriptSerializer.MaxJsonLength = Int32.MaxValue;
            if (flag != 0)
            {
                return Json("Request Already Sent", JsonRequestBehavior.AllowGet);
            }
            else
            {
                request.to_id = UserId;
                request.from_id = applicationUser.Id;
                ApplicationDb.Requests.Add(request);
                ApplicationDb.SaveChanges();
                
                return Json("Request Sent", JsonRequestBehavior.AllowGet);
            } 
        }

        public ActionResult Show()
        {
            ApplicationUser applicationUser = Session["User"] as ApplicationUser;
            List<Requests> requests = ApplicationDb.Requests.Where(r => r.to_id == applicationUser.Id && r.Accepted == false).ToList<Requests>();
            List<ApplicationUser> users=new List<ApplicationUser>();
            foreach(var req in requests)
            {
                var user=ApplicationDb.Users.Where(u => u.Id == req.from_id).First<ApplicationUser>();
                users.Add(user);
            }
            ViewBag.users = users;
            return View();
        }

        public ActionResult Accept(string UserId)
        {
            ApplicationUser applicationUser = Session["User"] as ApplicationUser;
            Requests req = ApplicationDb.Requests.Where(r => r.from_id == UserId && r.to_id == applicationUser.Id).First<Requests>();
            req.Accepted = true;
            ApplicationDb.SaveChanges();
            Notification notification = new Notification();
            notification.notificationType = NotificationType.Request;
            notification.Description=applicationUser.UserName + "Accepted Your Request";
            ApplicationUser nofuser = ApplicationDb.Users.Where(u => u.Id == UserId).FirstOrDefault();
            nofuser.Notifications.Add(notification);
            ApplicationDb.SaveChanges();
            return Json(Url.Action("Show", "Request"));
        }

        public ActionResult Decline(string UserId)
        {
            ApplicationUser applicationUser = Session["User"] as ApplicationUser;
            Requests req = ApplicationDb.Requests.Where(r => r.from_id == UserId && r.to_id == applicationUser.Id).First<Requests>();
            ApplicationDb.Requests.Remove(req);
            ApplicationDb.SaveChanges();
            Notification notification = new Notification();
            notification.notificationType = NotificationType.Request;
            notification.Description = applicationUser.UserName + "Declined Your Request";
            ApplicationUser nofuser = ApplicationDb.Users.Where(u => u.Id == UserId).FirstOrDefault();
            nofuser.Notifications.Add(notification);
            ApplicationDb.SaveChanges();
            return Json(Url.Action("Show", "Request"));
        }

        public ActionResult GetAllUserName()
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue;
            
            var usernames = ApplicationDb.Users.Select(u => u.UserName).ToArray<string>();
            return Json(javaScriptSerializer.Serialize(usernames), JsonRequestBehavior.AllowGet);
        }
    }

}