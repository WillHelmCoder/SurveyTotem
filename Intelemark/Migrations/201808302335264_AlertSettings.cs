namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlertSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlertSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificationEmails = c.String(),
                        DataPercentage = c.Double(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AlertSettings");
        }
    }
}
