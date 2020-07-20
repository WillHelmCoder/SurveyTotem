namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampaignUserHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserHistories", "CampaignId", c => c.Int());
            CreateIndex("dbo.UserHistories", "CampaignId");
            AddForeignKey("dbo.UserHistories", "CampaignId", "dbo.Campaigns", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserHistories", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.UserHistories", new[] { "CampaignId" });
            DropColumn("dbo.UserHistories", "CampaignId");
        }
    }
}
