namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FourFieldsInContact2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "Extension", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "Extension", c => c.Int(nullable: false));
        }
    }
}
