namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrioritiesEnProjects : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Priority", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Priority");
        }
    }
}
