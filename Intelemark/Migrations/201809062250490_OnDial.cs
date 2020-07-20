namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnDial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "OnDial", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "OnDial");
        }
    }
}
