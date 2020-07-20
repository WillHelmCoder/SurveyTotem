namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientToCategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "ClientId", "dbo.AspNetUsers");
            DropIndex("dbo.Categories", new[] { "ClientId" });
            AddColumn("dbo.Categories", "CampaignId", c => c.Int(nullable: false));
            CreateIndex("dbo.Categories", "CampaignId");
            AddForeignKey("dbo.Categories", "CampaignId", "dbo.Campaigns", "Id");
            DropColumn("dbo.Categories", "ClientId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "ClientId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Categories", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.Categories", new[] { "CampaignId" });
            DropColumn("dbo.Categories", "CampaignId");
            CreateIndex("dbo.Categories", "ClientId");
            AddForeignKey("dbo.Categories", "ClientId", "dbo.AspNetUsers", "Id");
        }
    }
}
