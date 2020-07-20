namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComputedSessionDuration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserHistories", "Duration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserHistories", "Duration", c => c.Double());
        }
    }
}
