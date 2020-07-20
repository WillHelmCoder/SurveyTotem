namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteNotifications : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "AppointmentsId", "dbo.Appointments");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "AppointmentsId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            AddColumn("dbo.Appointments", "Notified", c => c.Boolean(nullable: false));
            DropTable("dbo.Notifications");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScheduledDate = c.DateTime(nullable: false),
                        Notified = c.Boolean(nullable: false),
                        AppointmentsId = c.Int(),
                        UserId = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Appointments", "Notified");
            CreateIndex("dbo.Notifications", "UserId");
            CreateIndex("dbo.Notifications", "AppointmentsId");
            AddForeignKey("dbo.Notifications", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Notifications", "AppointmentsId", "dbo.Appointments", "Id");
        }
    }
}
