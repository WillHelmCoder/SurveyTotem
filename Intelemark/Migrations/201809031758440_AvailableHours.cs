namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AvailableHours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AvailableHours", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AvailableHours");
        }
    }
}
