namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectPriorities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectPriorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Field = c.String(),
                        PriorityValue = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.ProjectPriorityDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectPriorityId = c.Int(nullable: false),
                        FieldValue = c.String(),
                        FieldPriorityValue = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectPriorities", t => t.ProjectPriorityId)
                .Index(t => t.ProjectPriorityId);
            
            AddColumn("dbo.Contacts", "SIC", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectPriorityDetails", "ProjectPriorityId", "dbo.ProjectPriorities");
            DropForeignKey("dbo.ProjectPriorities", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectPriorityDetails", new[] { "ProjectPriorityId" });
            DropIndex("dbo.ProjectPriorities", new[] { "ProjectId" });
            DropColumn("dbo.Contacts", "SIC");
            DropTable("dbo.ProjectPriorityDetails");
            DropTable("dbo.ProjectPriorities");
        }
    }
}
