namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCampaignANDUserProject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCampaigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        CampaignId = c.Int(nullable: false),
                        PayRateTrainingHours = c.Double(nullable: false),
                        PayRateDialingHours = c.Double(nullable: false),
                        PayRateSuccess = c.Double(nullable: false),
                        BudgetedHours = c.Double(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "dbo.UserProjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ProjectId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProjects", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserProjects", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.UserCampaigns", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCampaigns", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.UserProjects", new[] { "ProjectId" });
            DropIndex("dbo.UserProjects", new[] { "UserId" });
            DropIndex("dbo.UserCampaigns", new[] { "CampaignId" });
            DropIndex("dbo.UserCampaigns", new[] { "UserId" });
            DropTable("dbo.UserProjects");
            DropTable("dbo.UserCampaigns");
        }
    }
}
