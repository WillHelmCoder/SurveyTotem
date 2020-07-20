namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsFinalizedInContacts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "IsFinalized", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "IsFinalized");
        }
    }
}
