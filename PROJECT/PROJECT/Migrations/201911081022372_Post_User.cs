namespace PROJECT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Post_User : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Posts", name: "UserId", newName: "User_Id");
            RenameIndex(table: "dbo.Posts", name: "IX_UserId", newName: "IX_User_Id");
            DropColumn("dbo.AspNetUsers", "PostId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "PostId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Posts", name: "IX_User_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.Posts", name: "User_Id", newName: "UserId");
        }
    }
}
