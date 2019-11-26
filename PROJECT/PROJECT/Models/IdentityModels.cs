using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PROJECT.Models
{
    public enum NotificationType
    {
        Comment = 1, Like ,Request, Post
    }

    public enum Gender
    {
        male=1,female
    }

    public class Photo
    {
        public int PhotoId { get; set; }
        [StringLength(255)]
        public string PhotoName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        //public PhotoType FileType { get; set; }
        
        //public string Id { get; set; }
        //[ForeignKey("Id")]
        //public virtual ApplicationUser User { get; set; }
    }

    public class Requests
    {
        public int RequestsId { get; set; }
        public string from_id { get; set; }
        public string to_id { get; set; }
        public bool Accepted { get; set; }
        [ForeignKey("from_id")]
        public virtual ApplicationUser FromUser { get; set; }
        [ForeignKey("to_id")]
        public virtual ApplicationUser ToUser { get; set; }

    }

    public class Post
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
        public int photoId { get; set; }
        //public int CommentId { get; set; }
        [Required]
        public string Caption { get; set; }
        public string LocationName { get; set; }
        public bool IsFriends { get; set; }
        public bool IsPublic { get; set; }
        public DateTime dateTime { get; set; }
        //public int Like { get; set; }


        //[ForeignKey("CommentId")]
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        [ForeignKey("photoId")]
        public virtual Photo Photo { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }

    public class Comment
    {
        public int CommentId { get; set; }
        //public int PostId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        //[ForeignKey("PostId")]
        public virtual Post Post { get; set; }
    }

    public class Notification
    {
        public int NotificationId { get; set; }
        public NotificationType notificationType { get; set; }
        public string Description { get; set; }
    }

    public class Like
    {
        public int LikeId { get; set; }
        public string UserId { get; set; } 

        [ForeignKey("UserId")]
        public ApplicationUser user { get; set; }
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Nick Name")]
        public string NickName { get; set; }
        [Required]
        [StringLength(255)]
        public string Bio { get; set; }

        public string ConnectionId { get; set; }

        [Required]
        public Gender Gender { get; set; }
        public int DpId { get; set; }
        //public int PostId { get; set; }
        [ForeignKey("DpId")]
        [Required]
        public virtual Photo Dp { get; set; }
        //[ForeignKey("PostId")]
        //public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Requests> Requests { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Like> Likes { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        } 
    }

    
}