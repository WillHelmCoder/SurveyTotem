namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Points", c => c.Int(nullable: false));
            AddColumn("dbo.Questions", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Image");
            DropColumn("dbo.Questions", "Points");
        }
    }
}
