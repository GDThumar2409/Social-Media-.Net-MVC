namespace PROJECT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class POST_COMMENT_3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Comments", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Comments", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Comments", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Comments", name: "UserId", newName: "User_Id");
        }
    }
}
