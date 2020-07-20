namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StateRestrictions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StateRestrictions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abbreviation = c.String(),
                        Name = c.String(),
                        IsRestricted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StateRestrictions");
        }
    }
}
