namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoriesToProjects : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Categories", newName: "Projects");
            RenameColumn(table: "dbo.Contacts", name: "CategoryId", newName: "ProjectId");
            RenameIndex(table: "dbo.Contacts", name: "IX_CategoryId", newName: "IX_ProjectId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Contacts", name: "IX_ProjectId", newName: "IX_CategoryId");
            RenameColumn(table: "dbo.Contacts", name: "ProjectId", newName: "CategoryId");
            RenameTable(name: "dbo.Projects", newName: "Categories");
        }
    }
}
