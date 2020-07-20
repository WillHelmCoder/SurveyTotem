namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgentLogAgentId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AgentLogs", "AgentId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AgentLogs", "AgentId");
            AddForeignKey("dbo.AgentLogs", "AgentId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AgentLogs", "AgentId", "dbo.AspNetUsers");
            DropIndex("dbo.AgentLogs", new[] { "AgentId" });
            DropColumn("dbo.AgentLogs", "AgentId");
        }
    }
}
