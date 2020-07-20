namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgentLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgentLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DialingHours = c.Double(nullable: false),
                        TrainingHours = c.Double(nullable: false),
                        Successes = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        TimeZoneId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.TimeZones", t => t.TimeZoneId)
                .Index(t => t.ProjectId)
                .Index(t => t.CampaignId)
                .Index(t => t.TimeZoneId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AgentLogs", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.AgentLogs", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.AgentLogs", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.AgentLogs", new[] { "TimeZoneId" });
            DropIndex("dbo.AgentLogs", new[] { "CampaignId" });
            DropIndex("dbo.AgentLogs", new[] { "ProjectId" });
            DropTable("dbo.AgentLogs");
        }
    }
}
