namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadeDeleteConvention : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AnsweredForms", "RecordId", "dbo.Records");
            DropForeignKey("dbo.Records", "CallCodeId", "dbo.CallCodes");
            DropForeignKey("dbo.Records", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.Records", "FormId", "dbo.Forms");
            DropForeignKey("dbo.Contacts", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Contacts", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Forms", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.Questions", "FormId", "dbo.Forms");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.AreaCodes", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ZipCodes", "TimeZoneId", "dbo.TimeZones");
            AddColumn("dbo.AnsweredForms", "QuestionId", c => c.Int(nullable: false));
            CreateIndex("dbo.AnsweredForms", "QuestionId");
            AddForeignKey("dbo.AnsweredForms", "QuestionId", "dbo.Questions", "Id");
            AddForeignKey("dbo.AnsweredForms", "RecordId", "dbo.Records", "Id");
            AddForeignKey("dbo.Records", "CallCodeId", "dbo.CallCodes", "Id");
            AddForeignKey("dbo.Records", "ContactId", "dbo.Contacts", "Id");
            AddForeignKey("dbo.Records", "FormId", "dbo.Forms", "Id");
            AddForeignKey("dbo.Contacts", "CategoryId", "dbo.Categories", "Id");
            AddForeignKey("dbo.Contacts", "TimeZoneId", "dbo.TimeZones", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Forms", "CampaignId", "dbo.Campaigns", "Id");
            AddForeignKey("dbo.Questions", "FormId", "dbo.Forms", "Id");
            AddForeignKey("dbo.Answers", "QuestionId", "dbo.Questions", "Id");
            AddForeignKey("dbo.AreaCodes", "TimeZoneId", "dbo.TimeZones", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.ZipCodes", "TimeZoneId", "dbo.TimeZones", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZipCodes", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AreaCodes", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Questions", "FormId", "dbo.Forms");
            DropForeignKey("dbo.Forms", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Contacts", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.Contacts", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Records", "FormId", "dbo.Forms");
            DropForeignKey("dbo.Records", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.Records", "CallCodeId", "dbo.CallCodes");
            DropForeignKey("dbo.AnsweredForms", "RecordId", "dbo.Records");
            DropForeignKey("dbo.AnsweredForms", "QuestionId", "dbo.Questions");
            DropIndex("dbo.AnsweredForms", new[] { "QuestionId" });
            DropColumn("dbo.AnsweredForms", "QuestionId");
            AddForeignKey("dbo.ZipCodes", "TimeZoneId", "dbo.TimeZones", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AreaCodes", "TimeZoneId", "dbo.TimeZones", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Answers", "QuestionId", "dbo.Questions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Questions", "FormId", "dbo.Forms", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Forms", "CampaignId", "dbo.Campaigns", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Contacts", "TimeZoneId", "dbo.TimeZones", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Contacts", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Records", "FormId", "dbo.Forms", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Records", "ContactId", "dbo.Contacts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Records", "CallCodeId", "dbo.CallCodes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AnsweredForms", "RecordId", "dbo.Records", "Id", cascadeDelete: true);
        }
    }
}
