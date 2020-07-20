namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScheduledDate = c.DateTime(nullable: false),
                        Notified = c.Boolean(nullable: false),
                        AppointmentsId = c.Int(),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Appointments", t => t.AppointmentsId)
                .Index(t => t.AppointmentsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "AppointmentsId", "dbo.Appointments");
            DropIndex("dbo.Notifications", new[] { "AppointmentsId" });
            DropTable("dbo.Notifications");
        }
    }
}
