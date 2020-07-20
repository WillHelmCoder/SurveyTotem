namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PenaltiesModule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Penalties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PayRate = c.Double(nullable: false),
                        From = c.Double(nullable: false),
                        To = c.Double(nullable: false),
                        PenaltyFee = c.Double(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Penalties");
        }
    }
}
