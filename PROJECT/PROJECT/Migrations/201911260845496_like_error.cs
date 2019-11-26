namespace PROJECT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class like_error : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Likes", name: "user_Id", newName: "UserId");
            RenameIndex(table: "dbo.Likes", name: "IX_user_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Likes", name: "IX_UserId", newName: "IX_user_Id");
            RenameColumn(table: "dbo.Likes", name: "UserId", newName: "user_Id");
        }
    }
}
