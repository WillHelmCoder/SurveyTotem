namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnswerQuestionv2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "AnswerName", c => c.String());
            AddColumn("dbo.Questions", "QuestionName", c => c.String());
            DropColumn("dbo.Answers", "Name");
            DropColumn("dbo.Questions", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "Name", c => c.String());
            AddColumn("dbo.Answers", "Name", c => c.String());
            DropColumn("dbo.Questions", "QuestionName");
            DropColumn("dbo.Answers", "AnswerName");
        }
    }
}
