namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InHold : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "InHold", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "InHold");
        }
    }
}
