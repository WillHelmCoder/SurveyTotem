namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactAttempts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Attempts", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "Attempts");
        }
    }
}
