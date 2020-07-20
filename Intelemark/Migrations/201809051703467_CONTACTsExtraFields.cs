namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CONTACTsExtraFields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExtraFieldOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldOptionName = c.String(),
                        ExtraFieldId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExtraFields", t => t.ExtraFieldId)
                .Index(t => t.ExtraFieldId);
            
            CreateTable(
                "dbo.ExtraFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldName = c.String(),
                        TypeId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "dbo.ExtraFieldValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExtraFieldId = c.Int(nullable: false),
                        Value = c.String(),
                        UserId = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExtraFields", t => t.ExtraFieldId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ExtraFieldId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExtraFieldValues", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ExtraFieldValues", "ExtraFieldId", "dbo.ExtraFields");
            DropForeignKey("dbo.ExtraFieldOptions", "ExtraFieldId", "dbo.ExtraFields");
            DropForeignKey("dbo.ExtraFields", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.ExtraFieldValues", new[] { "UserId" });
            DropIndex("dbo.ExtraFieldValues", new[] { "ExtraFieldId" });
            DropIndex("dbo.ExtraFields", new[] { "CampaignId" });
            DropIndex("dbo.ExtraFieldOptions", new[] { "ExtraFieldId" });
            DropTable("dbo.ExtraFieldValues");
            DropTable("dbo.ExtraFields");
            DropTable("dbo.ExtraFieldOptions");
        }
    }
}
