namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DurationFieldOnRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "Duration", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Records", "Duration");
        }
    }
}
