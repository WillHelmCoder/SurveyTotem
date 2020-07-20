namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationRecordAnsweredForm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnsweredForms", "RecordId", c => c.Int());
            CreateIndex("dbo.AnsweredForms", "RecordId");
            AddForeignKey("dbo.AnsweredForms", "RecordId", "dbo.Records", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnsweredForms", "RecordId", "dbo.Records");
            DropIndex("dbo.AnsweredForms", new[] { "RecordId" });
            DropColumn("dbo.AnsweredForms", "RecordId");
        }
    }
}
