namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CallbackReminder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "ReminderDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "ReminderDate");
        }
    }
}
