namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CallCodeRevision : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CallCodes", "IsSuccess", c => c.Boolean(nullable: false));
            AlterColumn("dbo.CallCodes", "Code", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CallCodes", "Code", c => c.Int(nullable: false));
            DropColumn("dbo.CallCodes", "IsSuccess");
        }
    }
}
