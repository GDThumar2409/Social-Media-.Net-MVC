namespace PROJECT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comment_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "notificationType", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "Description", c => c.String());
            DropColumn("dbo.Notifications", "Like");
            DropColumn("dbo.Notifications", "Comment");
            DropColumn("dbo.Notifications", "Request");
            DropColumn("dbo.Notifications", "Post");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "Post", c => c.String());
            AddColumn("dbo.Notifications", "Request", c => c.String());
            AddColumn("dbo.Notifications", "Comment", c => c.String());
            AddColumn("dbo.Notifications", "Like", c => c.String());
            DropColumn("dbo.Notifications", "Description");
            DropColumn("dbo.Notifications", "notificationType");
        }
    }
}
