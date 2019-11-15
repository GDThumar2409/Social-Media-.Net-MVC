using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PROJECT.Models;
namespace PROJECT.Controllers
{
    
    public class ProfileController : Controller
    {
        ApplicationDbContext ApplicationDb = new ApplicationDbContext();
        // GET: Profile
        public ActionResult Index(string id)
        {
            ApplicationUser applicationuser = ApplicationDb.Users.Where(u => u.Id == id).FirstOrDefault<ApplicationUser>();
            List<Post> allposts = ApplicationDb.Posts.ToList<Post>();
            var posts = from p in allposts                    
                        where p.UserId == id
                        orderby p.dateTime descending
                        select p;
           
            ViewBag.user = applicationuser;
            ViewBag.posts = posts;
         
            return View();
        }

        public ActionResult File(string id)
        {
            ApplicationUser applicationuser = ApplicationDb.Users.Where(u => u.Id == id).FirstOrDefault<ApplicationUser>();
            //var fileToRetrieve = user.Dp;
            Photo photo = applicationuser.Dp;
            return File(photo.Content, photo.ContentType);
        }

        public ActionResult Post(int id)
        {

            //var fileToRetrieve = user.Dp;
            Photo photo = ApplicationDb.Photos.Where(p => p.PhotoId == id).FirstOrDefault<Photo>();
            return File(photo.Content, photo.ContentType);
        }
    }
}