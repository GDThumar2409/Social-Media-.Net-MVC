namespace PROJECT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Like : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        LikeId = c.Int(nullable: false, identity: true),
                        user_Id = c.String(maxLength: 128),
                        Post_PostId = c.Int(),
                    })
                .PrimaryKey(t => t.LikeId)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .ForeignKey("dbo.Posts", t => t.Post_PostId)
                .Index(t => t.user_Id)
                .Index(t => t.Post_PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "Post_PostId", "dbo.Posts");
            DropForeignKey("dbo.Likes", "user_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Likes", new[] { "Post_PostId" });
            DropIndex("dbo.Likes", new[] { "user_Id" });
            DropTable("dbo.Likes");
        }
    }
}
