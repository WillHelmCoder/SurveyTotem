namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationRecordAnsweredFormNonNull : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AnsweredForms", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.AnsweredForms", "RecordId", "dbo.Records");
            DropIndex("dbo.AnsweredForms", new[] { "QuestionId" });
            DropIndex("dbo.AnsweredForms", new[] { "RecordId" });
            AlterColumn("dbo.AnsweredForms", "RecordId", c => c.Int(nullable: false));
            CreateIndex("dbo.AnsweredForms", "RecordId");
            AddForeignKey("dbo.AnsweredForms", "RecordId", "dbo.Records", "Id", cascadeDelete: true);
            DropColumn("dbo.AnsweredForms", "QuestionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AnsweredForms", "QuestionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.AnsweredForms", "RecordId", "dbo.Records");
            DropIndex("dbo.AnsweredForms", new[] { "RecordId" });
            AlterColumn("dbo.AnsweredForms", "RecordId", c => c.Int());
            CreateIndex("dbo.AnsweredForms", "RecordId");
            CreateIndex("dbo.AnsweredForms", "QuestionId");
            AddForeignKey("dbo.AnsweredForms", "RecordId", "dbo.Records", "Id");
            AddForeignKey("dbo.AnsweredForms", "QuestionId", "dbo.Questions", "Id", cascadeDelete: true);
        }
    }
}
