namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeZoneIdEnStates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StateRestrictions", "TimeZoneId", c => c.Int());
            CreateIndex("dbo.StateRestrictions", "TimeZoneId");
            AddForeignKey("dbo.StateRestrictions", "TimeZoneId", "dbo.TimeZones", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StateRestrictions", "TimeZoneId", "dbo.TimeZones");
            DropIndex("dbo.StateRestrictions", new[] { "TimeZoneId" });
            DropColumn("dbo.StateRestrictions", "TimeZoneId");
        }
    }
}
