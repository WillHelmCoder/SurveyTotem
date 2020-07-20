namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnWatchSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OnWatchSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(nullable: false),
                        HoursLeft = c.Double(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .Index(t => t.CampaignId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OnWatchSettings", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.OnWatchSettings", new[] { "CampaignId" });
            DropTable("dbo.OnWatchSettings");
        }
    }
}
