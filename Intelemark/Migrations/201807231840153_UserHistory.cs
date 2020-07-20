namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        LoggedIn = c.DateTime(nullable: false),
                        LoggedOff = c.DateTime(nullable: false),
                        Duration = c.Double(),
                        ConnectionId = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserHistories", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserHistories", new[] { "UserId" });
            DropTable("dbo.UserHistories");
        }
    }
}
