namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeZonesIds : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AreaCodes", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.ZipCodes", "TimeZoneId", "dbo.TimeZones");
            DropIndex("dbo.AreaCodes", new[] { "TimeZoneId" });
            DropIndex("dbo.ZipCodes", new[] { "TimeZoneId" });
            AlterColumn("dbo.AreaCodes", "TimeZoneId", c => c.Int(nullable: false));
            AlterColumn("dbo.ZipCodes", "TimeZoneId", c => c.Int(nullable: false));
            CreateIndex("dbo.AreaCodes", "TimeZoneId");
            CreateIndex("dbo.ZipCodes", "TimeZoneId");
            AddForeignKey("dbo.AreaCodes", "TimeZoneId", "dbo.TimeZones", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ZipCodes", "TimeZoneId", "dbo.TimeZones", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZipCodes", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.AreaCodes", "TimeZoneId", "dbo.TimeZones");
            DropIndex("dbo.ZipCodes", new[] { "TimeZoneId" });
            DropIndex("dbo.AreaCodes", new[] { "TimeZoneId" });
            AlterColumn("dbo.ZipCodes", "TimeZoneId", c => c.Int());
            AlterColumn("dbo.AreaCodes", "TimeZoneId", c => c.Int());
            CreateIndex("dbo.ZipCodes", "TimeZoneId");
            CreateIndex("dbo.AreaCodes", "TimeZoneId");
            AddForeignKey("dbo.ZipCodes", "TimeZoneId", "dbo.TimeZones", "Id");
            AddForeignKey("dbo.AreaCodes", "TimeZoneId", "dbo.TimeZones", "Id");
        }
    }
}
