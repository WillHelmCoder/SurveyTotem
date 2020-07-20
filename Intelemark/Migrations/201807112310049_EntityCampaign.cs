namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntityCampaign : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Objective = c.String(),
                        BillingPH = c.String(),
                        CompanyLink = c.Boolean(nullable: false),
                        ActiveControl = c.Boolean(nullable: false),
                        SpellCheck = c.Boolean(nullable: false),
                        MaxAttempt = c.Int(nullable: false),
                        AccountManagerId = c.String(maxLength: 128),
                        ClientId = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AccountManagerId)
                .ForeignKey("dbo.AspNetUsers", t => t.ClientId)
                .Index(t => t.AccountManagerId)
                .Index(t => t.ClientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaigns", "ClientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Campaigns", "AccountManagerId", "dbo.AspNetUsers");
            DropIndex("dbo.Campaigns", new[] { "ClientId" });
            DropIndex("dbo.Campaigns", new[] { "AccountManagerId" });
            DropTable("dbo.Campaigns");
        }
    }
}
