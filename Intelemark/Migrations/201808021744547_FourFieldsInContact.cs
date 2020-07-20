namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FourFieldsInContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Extension", c => c.Int(nullable: false));
            AddColumn("dbo.Contacts", "Title", c => c.String());
            AddColumn("dbo.Contacts", "Company", c => c.String());
            AddColumn("dbo.Contacts", "AltAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "AltAddress");
            DropColumn("dbo.Contacts", "Company");
            DropColumn("dbo.Contacts", "Title");
            DropColumn("dbo.Contacts", "Extension");
        }
    }
}
