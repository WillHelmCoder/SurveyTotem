namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crimes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExtraFieldValues", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.ExtraFieldOptions", "ExtraFieldId", "dbo.ExtraFields");
            DropForeignKey("dbo.ExtraFieldValues", "ExtraFieldId", "dbo.ExtraFields");
            DropIndex("dbo.ExtraFieldValues", new[] { "ExtraFieldId" });
            DropIndex("dbo.ExtraFieldValues", new[] { "ContactId" });
            DropIndex("dbo.ExtraFieldOptions", new[] { "ExtraFieldId" });
            DropTable("dbo.ExtraFieldValues");
            DropTable("dbo.ExtraFieldOptions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ExtraFieldOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldOptionName = c.String(),
                        ExtraFieldId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExtraFieldValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExtraFieldId = c.Int(nullable: false),
                        Value = c.String(),
                        ContactId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ExtraFieldOptions", "ExtraFieldId");
            CreateIndex("dbo.ExtraFieldValues", "ContactId");
            CreateIndex("dbo.ExtraFieldValues", "ExtraFieldId");
            AddForeignKey("dbo.ExtraFieldValues", "ExtraFieldId", "dbo.ExtraFields", "Id");
            AddForeignKey("dbo.ExtraFieldOptions", "ExtraFieldId", "dbo.ExtraFields", "Id");
            AddForeignKey("dbo.ExtraFieldValues", "ContactId", "dbo.Contacts", "Id");
        }
    }
}
