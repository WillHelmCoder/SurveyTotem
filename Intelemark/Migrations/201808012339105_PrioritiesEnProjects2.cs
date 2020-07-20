namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrioritiesEnProjects2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Priority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Priority", c => c.Int());
        }
    }
}
