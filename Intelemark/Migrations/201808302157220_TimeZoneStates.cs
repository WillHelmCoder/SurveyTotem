namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeZoneStates : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.StateRestrictions", new[] { "TimeZoneId" });
            AlterColumn("dbo.StateRestrictions", "TimeZoneId", c => c.Int(nullable: false));
            CreateIndex("dbo.StateRestrictions", "TimeZoneId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.StateRestrictions", new[] { "TimeZoneId" });
            AlterColumn("dbo.StateRestrictions", "TimeZoneId", c => c.Int());
            CreateIndex("dbo.StateRestrictions", "TimeZoneId");
        }
    }
}
