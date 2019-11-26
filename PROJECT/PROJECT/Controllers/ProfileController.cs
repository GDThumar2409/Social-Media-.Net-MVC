using PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PROJECT.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        ApplicationDbContext ApplicationDb = new ApplicationDbContext();
        // GET: Profile
        public ActionResult Index(string id)
        {
            ApplicationUser applicationuser = ApplicationDb.Users.Where(u => u.Id == id).FirstOrDefault<ApplicationUser>();
            int count = ApplicationDb.Requests.Where(r => r.to_id == applicationuser.Id && r.Accepted == true).Count();
            int following = ApplicationDb.Requests.Where(r => r.from_id == applicationuser.Id && r.Accepted == true).Count();
            List<Post> allposts = ApplicationDb.Posts.ToList<Post>();
            List<Post> posts = (from p in allposts
                        where p.UserId == id
                        orderby p.dateTime descending
                        select p).ToList();

            ViewBag.user = applicationuser;
            ViewBag.posts = posts;
            ViewBag.followings = following;
            ViewBag.counts = count;


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


        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = ApplicationDb.Users.Where(u => u.Id == id).FirstOrDefault<ApplicationUser>();

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(HttpPostedFileBase upload)
        {
            ApplicationUser user = Session["User"] as ApplicationUser;
            string id = user.Id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var UserToUpdate = ApplicationDb.Users.Find(id);
            if (TryUpdateModel(UserToUpdate, "",
                new string[] { "Bio" ,"Gender"}))
            {
                try
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        if (UserToUpdate.Dp != null)
                        {
                            Photo dp = UserToUpdate.Dp;
                            UserToUpdate.Dp = null;
                            ApplicationDb.Photos.Remove(dp);
                            //UserToUpdate.Dp = null;
                        }
                        Photo avatar = new Photo
                        {
                            PhotoName = System.IO.Path.GetFileName(upload.FileName),
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            avatar.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        UserToUpdate.Dp = avatar;
                    }
                    //  ApplicationDb.Entry(UserToUpdate).State = EntityState.Modified;
                    ApplicationDb.SaveChanges();

                    return RedirectToAction("Index", new { id = id });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(UserToUpdate);
        }

        public ActionResult Unfriend(string id)
        {
            ApplicationUser user = Session["User"] as ApplicationUser;
            Requests req = ApplicationDb.Requests.Where(r => r.from_id == user.Id && r.to_id == id).FirstOrDefault<Requests>();
            ApplicationDb.Requests.Remove(req);
            ApplicationDb.SaveChanges();
            return RedirectToAction("index","home");
        }
    }
}