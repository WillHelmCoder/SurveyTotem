namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreationDateEnUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastUpdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsDeleted");
            DropColumn("dbo.AspNetUsers", "LastUpdate");
            DropColumn("dbo.AspNetUsers", "CreationDate");
        }
    }
}
