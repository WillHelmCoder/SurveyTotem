namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationRemoveContactIdAnsweredForm : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AnsweredForms", "Record_Id", "dbo.Records");
            DropIndex("dbo.AnsweredForms", new[] { "Record_Id" });
            DropColumn("dbo.AnsweredForms", "Record_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AnsweredForms", "Record_Id", c => c.Int());
            CreateIndex("dbo.AnsweredForms", "Record_Id");
            AddForeignKey("dbo.AnsweredForms", "Record_Id", "dbo.Records", "Id");
        }
    }
}
