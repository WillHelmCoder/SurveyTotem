namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appointmens : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Notes = c.String(),
                        DateScheduled = c.DateTime(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        RecordId = c.Int(),
                        AgentId = c.Int(nullable: false),
                        IsConfirmed = c.Boolean(nullable: false),
                        IsScheduled = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Agent_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Agent_Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.Records", t => t.RecordId)
                .Index(t => t.CampaignId)
                .Index(t => t.RecordId)
                .Index(t => t.Agent_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "RecordId", "dbo.Records");
            DropForeignKey("dbo.Appointments", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.Appointments", "Agent_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "Agent_Id" });
            DropIndex("dbo.Appointments", new[] { "RecordId" });
            DropIndex("dbo.Appointments", new[] { "CampaignId" });
            DropTable("dbo.Appointments");
        }
    }
}
