namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FGP : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Records", new[] { "FormId" });
            AlterColumn("dbo.Records", "FormId", c => c.Int());
            CreateIndex("dbo.Records", "FormId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Records", new[] { "FormId" });
            AlterColumn("dbo.Records", "FormId", c => c.Int(nullable: false));
            CreateIndex("dbo.Records", "FormId");
        }
    }
}
