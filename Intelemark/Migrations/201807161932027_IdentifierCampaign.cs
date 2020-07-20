namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentifierCampaign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "Identifier", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "Identifier");
        }
    }
}
