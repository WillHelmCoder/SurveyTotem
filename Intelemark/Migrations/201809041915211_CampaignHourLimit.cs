namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampaignHourLimit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "CampaignLimit", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "CampaignLimit");
        }
    }
}
