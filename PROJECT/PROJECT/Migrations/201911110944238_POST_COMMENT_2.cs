namespace PROJECT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class POST_COMMENT_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropIndex("dbo.Comments", new[] { "PostId" });
            RenameColumn(table: "dbo.Comments", name: "PostId", newName: "Post_PostId");
            RenameColumn(table: "dbo.Comments", name: "UserId", newName: "User_Id");
            RenameIndex(table: "dbo.Comments", name: "IX_UserId", newName: "IX_User_Id");
            AlterColumn("dbo.Comments", "Post_PostId", c => c.Int());
            CreateIndex("dbo.Comments", "Post_PostId");
            AddForeignKey("dbo.Comments", "Post_PostId", "dbo.Posts", "PostId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Post_PostId", "dbo.Posts");
            DropIndex("dbo.Comments", new[] { "Post_PostId" });
            AlterColumn("dbo.Comments", "Post_PostId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Comments", name: "IX_User_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.Comments", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Comments", name: "Post_PostId", newName: "PostId");
            CreateIndex("dbo.Comments", "PostId");
            AddForeignKey("dbo.Comments", "PostId", "dbo.Posts", "PostId", cascadeDelete: true);
        }
    }
}
