namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeZoneZipCodes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ClientId = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ClientId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNumber = c.String(),
                        AltPhoneNumber = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        ZipCode = c.String(),
                        CategoryId = c.Int(nullable: false),
                        TimeZoneId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.TimeZones", t => t.TimeZoneId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.TimeZoneId);
            
            CreateTable(
                "dbo.TimeZones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        STD = c.Int(nullable: false),
                        DST = c.Int(nullable: false),
                        CurrentTime = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ZipCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        STD = c.Int(nullable: false),
                        DST = c.Int(nullable: false),
                        TimeZoneId = c.Int(),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimeZones", t => t.TimeZoneId)
                .Index(t => t.TimeZoneId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZipCodes", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.Contacts", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.Contacts", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "ClientId", "dbo.AspNetUsers");
            DropIndex("dbo.ZipCodes", new[] { "TimeZoneId" });
            DropIndex("dbo.Contacts", new[] { "TimeZoneId" });
            DropIndex("dbo.Contacts", new[] { "CategoryId" });
            DropIndex("dbo.Categories", new[] { "ClientId" });
            DropTable("dbo.ZipCodes");
            DropTable("dbo.TimeZones");
            DropTable("dbo.Contacts");
            DropTable("dbo.Categories");
        }
    }
}
