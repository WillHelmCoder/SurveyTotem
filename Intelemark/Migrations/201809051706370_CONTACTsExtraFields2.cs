namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CONTACTsExtraFields2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExtraFieldValues", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ExtraFieldValues", new[] { "UserId" });
            AddColumn("dbo.ExtraFieldValues", "ContactId", c => c.Int(nullable: false));
            CreateIndex("dbo.ExtraFieldValues", "ContactId");
            AddForeignKey("dbo.ExtraFieldValues", "ContactId", "dbo.Contacts", "Id");
            DropColumn("dbo.ExtraFieldValues", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExtraFieldValues", "UserId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ExtraFieldValues", "ContactId", "dbo.Contacts");
            DropIndex("dbo.ExtraFieldValues", new[] { "ContactId" });
            DropColumn("dbo.ExtraFieldValues", "ContactId");
            CreateIndex("dbo.ExtraFieldValues", "UserId");
            AddForeignKey("dbo.ExtraFieldValues", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
