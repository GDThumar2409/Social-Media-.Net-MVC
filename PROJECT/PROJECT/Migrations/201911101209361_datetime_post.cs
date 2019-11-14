namespace PROJECT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datetime_post : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "dateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "dateTime");
        }
    }
}
