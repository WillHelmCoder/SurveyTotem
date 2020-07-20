namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotesEnContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "Notes");
        }
    }
}
