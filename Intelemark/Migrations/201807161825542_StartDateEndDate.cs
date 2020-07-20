namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StartDateEndDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "StartDate", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "EndDate");
            DropColumn("dbo.AspNetUsers", "StartDate");
        }
    }
}
