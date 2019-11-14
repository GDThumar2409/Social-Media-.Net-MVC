using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PROJECT.Models;

namespace PROJECT.Controllers
{
    
    public class HomeController : Controller
    {
        public ApplicationDbContext applicationDb=new ApplicationDbContext();
        //public SocialMediaDbContext SocialMediaDb = new SocialMediaDbContext();

        public ActionResult Index()
        {
            ApplicationUser applicationUser = Session["User"] as ApplicationUser;
            List<ApplicationUser> friends= new List<ApplicationUser>();
            ApplicationUser user;
            int reqcount = applicationDb.Requests.Where(r => r.to_id == applicationUser.Id && r.Accepted == false).Count();
            List<Requests> requests  = applicationDb.Requests.Where(r => r.to_id == applicationUser.Id && r.Accepted == true).ToList<Requests>();
            foreach(Requests req in requests)
            {
                user = applicationDb.Users.Where(u => u.Id == req.from_id).First<ApplicationUser>();
                friends.Add(user);
            }
            ViewBag.Friends = friends;
            ViewBag.reqcount = reqcount;
            List<Post> allposts = applicationDb.Posts.ToList<Post>();
            //List<Post> userpost = new List<Post>();
            //foreach(var post in allposts)
            //{
            //  if(post.UserId== applicationUser.Id)
            //{
            //  userpost.Add(post);
            //}

            //}
           // var pts=from p in applicationDb.Posts
             //       from  f in friends
               //     where p.UserId == f.Id
                 //   orderby p.dateTime descending
                   // select p;

            var posts = from p in allposts
                        from f in friends
                        where p.UserId == f.Id
                        orderby p.dateTime descending
                        select p;
            ViewBag.posts = posts;
            return View();
        }

        public ActionResult Comment(string comment,int postid)
        {
            ApplicationUser applicationUser = Session["User"] as ApplicationUser;
            Comment com = new Comment();
            com.Description = comment;
            
            
            //ApplicationDbContext applicationDb2 = new ApplicationDbContext();
            //Post post = applicationDb2.Posts.Where(p => p.PostId == postid).First<Post>();
            //post.Comments.Add(com);
            //com.Post = post;
            Comment cmt=applicationDb.Comments.Add(com);
            applicationDb.SaveChanges();
            cmt.Post = applicationDb.Posts.Where(p => p.PostId == postid).First<Post>();
            cmt.UserId = applicationUser.Id;
            applicationDb.SaveChanges();
            
            //applicationDb.SaveChanges();
            // post.Comments.Add(cmt);
            //applicationDb2.SaveChanges();

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult File(int id)
        {
            Photo photo = applicationDb.Photos.Where(p => p.PhotoId == id).First<Photo>();
            //var fileToRetrieve = user.Dp;
            return File(photo.Content, photo.ContentType);
        }

        public ActionResult Post(string location,string description,HttpPostedFileBase upload)
        {
            Post post = new Post();
            post.Caption = description;
            post.LocationName = location;
            Photo avatar;
            if (upload != null && upload.ContentLength > 0)
            {
                avatar = new Photo
                {
                    PhotoName = System.IO.Path.GetFileName(upload.FileName),
                    ContentType = upload.ContentType
                };
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    avatar.Content = reader.ReadBytes(upload.ContentLength);
                }
                try
                {
                    ApplicationUser applicationUser = Session["User"] as ApplicationUser;
                    post.Photo = avatar;
                    post.dateTime = DateTime.Now;
                    //applicationDb.Photos.Add(avatar);
                    post.UserId = applicationUser.Id;
                    //applicationUser.Posts.Add(post);
                    applicationDb.Posts.Add(post);
                    
                    //applicationUser.Posts.Add(post);
                    applicationDb.SaveChanges();
                    //applicationUser.Posts.Add(post);
                    //applicationDb.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }

                
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}