namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnswerQuestionv3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "Name", c => c.String());
            AddColumn("dbo.Questions", "Name", c => c.String());
            DropColumn("dbo.Answers", "AnswerName");
            DropColumn("dbo.Questions", "QuestionName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "QuestionName", c => c.String());
            AddColumn("dbo.Answers", "AnswerName", c => c.String());
            DropColumn("dbo.Questions", "Name");
            DropColumn("dbo.Answers", "Name");
        }
    }
}
