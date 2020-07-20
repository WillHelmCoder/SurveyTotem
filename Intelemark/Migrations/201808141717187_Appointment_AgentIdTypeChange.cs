namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appointment_AgentIdTypeChange : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Appointments", new[] { "Agent_Id" });
            DropColumn("dbo.Appointments", "AgentId");
            RenameColumn(table: "dbo.Appointments", name: "Agent_Id", newName: "AgentId");
            AlterColumn("dbo.Appointments", "AgentId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Appointments", "AgentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Appointments", new[] { "AgentId" });
            AlterColumn("dbo.Appointments", "AgentId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Appointments", name: "AgentId", newName: "Agent_Id");
            AddColumn("dbo.Appointments", "AgentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Appointments", "Agent_Id");
        }
    }
}
