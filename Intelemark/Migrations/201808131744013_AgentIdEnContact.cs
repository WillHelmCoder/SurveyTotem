namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgentIdEnContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "AgentId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Contacts", "AgentId");
            AddForeignKey("dbo.Contacts", "AgentId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "AgentId", "dbo.AspNetUsers");
            DropIndex("dbo.Contacts", new[] { "AgentId" });
            DropColumn("dbo.Contacts", "AgentId");
        }
    }
}
