namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LigarUseryPerfil : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserProfileId", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfiles", "ApplicationUserId", c => c.String(maxLength: 128));
            AddForeignKey("dbo.UserProfiles", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.UserProfiles", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfiles", "UserId", c => c.String());
            DropForeignKey("dbo.UserProfiles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserProfiles", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.UserProfiles", "ApplicationUser_Id");
            DropColumn("dbo.UserProfiles", "ApplicationUserId");
            DropColumn("dbo.AspNetUsers", "UserProfileId");
        }
    }
}
