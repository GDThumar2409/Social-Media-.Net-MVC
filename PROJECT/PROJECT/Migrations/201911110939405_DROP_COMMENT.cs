namespace PROJECT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DROP_COMMENT : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "CommentId", "dbo.Posts");
            DropIndex("dbo.Comments", new[] { "CommentId" });
            DropTable("dbo.Comments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        UserId = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CommentId);
            
            CreateIndex("dbo.Comments", "CommentId");
            AddForeignKey("dbo.Comments", "CommentId", "dbo.Posts", "PostId");
        }
    }
}
