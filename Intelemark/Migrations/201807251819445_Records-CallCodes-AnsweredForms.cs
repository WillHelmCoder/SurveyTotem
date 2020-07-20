namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecordsCallCodesAnsweredForms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnsweredForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContactId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        Answer = c.String(),
                        UserId = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.ContactId, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ContactId)
                .Index(t => t.QuestionId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CallCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.Int(nullable: false),
                        Behavior = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Records",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ContactId = c.Int(nullable: false),
                        FormId = c.Int(nullable: false),
                        CallCodeId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CallCodes", t => t.CallCodeId, cascadeDelete: true)
                .ForeignKey("dbo.Contacts", t => t.ContactId, cascadeDelete: true)
                .ForeignKey("dbo.Forms", t => t.FormId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ContactId)
                .Index(t => t.FormId)
                .Index(t => t.CallCodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Records", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Records", "FormId", "dbo.Forms");
            DropForeignKey("dbo.Records", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.Records", "CallCodeId", "dbo.CallCodes");
            DropForeignKey("dbo.AnsweredForms", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AnsweredForms", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.AnsweredForms", "ContactId", "dbo.Contacts");
            DropIndex("dbo.Records", new[] { "CallCodeId" });
            DropIndex("dbo.Records", new[] { "FormId" });
            DropIndex("dbo.Records", new[] { "ContactId" });
            DropIndex("dbo.Records", new[] { "UserId" });
            DropIndex("dbo.AnsweredForms", new[] { "UserId" });
            DropIndex("dbo.AnsweredForms", new[] { "QuestionId" });
            DropIndex("dbo.AnsweredForms", new[] { "ContactId" });
            DropTable("dbo.Records");
            DropTable("dbo.CallCodes");
            DropTable("dbo.AnsweredForms");
        }
    }
}
