namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCampaignenUserProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProjects", "UserCampaignId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserProjects", "UserCampaignId");
            AddForeignKey("dbo.UserProjects", "UserCampaignId", "dbo.UserCampaigns", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProjects", "UserCampaignId", "dbo.UserCampaigns");
            DropIndex("dbo.UserProjects", new[] { "UserCampaignId" });
            DropColumn("dbo.UserProjects", "UserCampaignId");
        }
    }
}
