namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DurationUserHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserHistories", "Duration", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserHistories", "Duration");
        }
    }
}
