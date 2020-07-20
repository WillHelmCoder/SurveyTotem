namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampaignCallCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignCallCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(nullable: false),
                        CallCodeId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CallCodes", t => t.CallCodeId)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .Index(t => t.CampaignId)
                .Index(t => t.CallCodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CampaignCallCodes", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.CampaignCallCodes", "CallCodeId", "dbo.CallCodes");
            DropIndex("dbo.CampaignCallCodes", new[] { "CallCodeId" });
            DropIndex("dbo.CampaignCallCodes", new[] { "CampaignId" });
            DropTable("dbo.CampaignCallCodes");
        }
    }
}
