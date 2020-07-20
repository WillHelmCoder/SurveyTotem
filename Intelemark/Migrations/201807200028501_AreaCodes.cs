namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AreaCodes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AreaCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TZ = c.String(),
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
            DropForeignKey("dbo.AreaCodes", "TimeZoneId", "dbo.TimeZones");
            DropIndex("dbo.AreaCodes", new[] { "TimeZoneId" });
            DropTable("dbo.AreaCodes");
        }
    }
}
