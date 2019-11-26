namespace PROJECT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notification2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Like", c => c.String());
            AddColumn("dbo.Notifications", "Comment", c => c.String());
            AddColumn("dbo.Notifications", "Request", c => c.String());
            AddColumn("dbo.Notifications", "Post", c => c.String());
            DropColumn("dbo.Notifications", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "Description", c => c.String());
            DropColumn("dbo.Notifications", "Post");
            DropColumn("dbo.Notifications", "Request");
            DropColumn("dbo.Notifications", "Comment");
            DropColumn("dbo.Notifications", "Like");
        }
    }
}
