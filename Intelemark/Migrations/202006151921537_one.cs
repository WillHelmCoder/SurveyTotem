namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class one : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgentLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DialingHours = c.Double(nullable: false),
                        TrainingHours = c.Double(nullable: false),
                        Successes = c.Int(nullable: false),
                        AgentId = c.String(maxLength: 128),
                        ProjectId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        TimeZoneId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AgentId)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.TimeZones", t => t.TimeZoneId)
                .Index(t => t.AgentId)
                .Index(t => t.ProjectId)
                .Index(t => t.CampaignId)
                .Index(t => t.TimeZoneId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Contact = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        ZipCode = c.String(),
                        Notes = c.String(),
                        AvailableHours = c.Double(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNumber = c.String(),
                        AltPhoneNumber = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        ZipCode = c.String(),
                        Notes = c.String(),
                        Extension = c.Int(),
                        Title = c.String(),
                        SIC = c.String(),
                        Company = c.String(),
                        AltAddress = c.String(),
                        Attempts = c.Int(nullable: false),
                        IsFinalized = c.Boolean(nullable: false),
                        InHold = c.Boolean(nullable: false),
                        OnDial = c.Boolean(nullable: false),
                        LeadId = c.Int(nullable: false),
                        TimeZoneId = c.Int(nullable: false),
                        AgentId = c.String(maxLength: 128),
                        ProjectId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AgentId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.TimeZones", t => t.TimeZoneId)
                .Index(t => t.TimeZoneId)
                .Index(t => t.AgentId)
                .Index(t => t.ProjectId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .ForeignKey("dbo.ExtraFields", t => t.ExtraFieldId)
                .Index(t => t.ExtraFieldId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.ExtraFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldName = c.String(),
                        TypeId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identifier = c.String(maxLength: 10),
                        Description = c.String(),
                        Objective = c.String(),
                        BillingPH = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CompanyLink = c.Boolean(nullable: false),
                        ActiveControl = c.Boolean(nullable: false),
                        SpellCheck = c.Boolean(nullable: false),
                        MaxAttempt = c.Int(nullable: false),
                        CampaignLimit = c.Double(nullable: false),
                        AccountManagerId = c.String(maxLength: 128),
                        ClientId = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AccountManagerId)
                .ForeignKey("dbo.AspNetUsers", t => t.ClientId)
                .Index(t => t.AccountManagerId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Forms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CampaignId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TypeId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        FormId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Forms", t => t.FormId)
                .Index(t => t.FormId);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Order = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Priority = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .Index(t => t.CampaignId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExtraFields", t => t.ExtraFieldId)
                .Index(t => t.ExtraFieldId);
            
            CreateTable(
                "dbo.TimeZones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        STD = c.Int(nullable: false),
                        DST = c.Int(nullable: false),
                        CurrentTime = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Records",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Duration = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ContactId = c.Int(nullable: false),
                        FormId = c.Int(),
                        CallCodeId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CallCodes", t => t.CallCodeId)
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .ForeignKey("dbo.Forms", t => t.FormId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ContactId)
                .Index(t => t.FormId)
                .Index(t => t.CallCodeId);
            
            CreateTable(
                "dbo.AnsweredForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Answer = c.String(),
                        QuestionId = c.Int(nullable: false),
                        RecordId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .ForeignKey("dbo.Records", t => t.RecordId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.QuestionId)
                .Index(t => t.RecordId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CallCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        IsSuccess = c.Boolean(nullable: false),
                        Behavior = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AlertSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificationEmails = c.String(),
                        DataPercentage = c.Double(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Notes = c.String(),
                        DateScheduled = c.DateTime(nullable: false),
                        ReminderDate = c.DateTime(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        RecordId = c.Int(),
                        AgentId = c.String(maxLength: 128),
                        IsConfirmed = c.Boolean(nullable: false),
                        Notified = c.Boolean(nullable: false),
                        TimesScheduled = c.Int(nullable: false),
                        IsScheduled = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AgentId)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.Records", t => t.RecordId)
                .Index(t => t.CampaignId)
                .Index(t => t.RecordId)
                .Index(t => t.AgentId);
            
            CreateTable(
                "dbo.AreaCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TZ = c.String(),
                        TimeZoneId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimeZones", t => t.TimeZoneId)
                .Index(t => t.TimeZoneId);
            
            CreateTable(
                "dbo.CampaignCallCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(nullable: false),
                        CallCodeId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CallCodes", t => t.CallCodeId)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .Index(t => t.CampaignId)
                .Index(t => t.CallCodeId);
            
            CreateTable(
                "dbo.ExportSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(nullable: false),
                        Name = c.String(),
                        Key = c.String(),
                        Value = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OnWatchSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(nullable: false),
                        HoursLeft = c.Double(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "dbo.Penalties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PayRate = c.Double(nullable: false),
                        From = c.Double(nullable: false),
                        To = c.Double(nullable: false),
                        PenaltyFee = c.Double(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.StateRestrictions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abbreviation = c.String(),
                        Name = c.String(),
                        IsRestricted = c.Boolean(nullable: false),
                        TimeZoneId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimeZones", t => t.TimeZoneId)
                .Index(t => t.TimeZoneId);
            
            CreateTable(
                "dbo.UserCampaigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        CampaignId = c.Int(nullable: false),
                        PayRateTrainingHours = c.Double(nullable: false),
                        PayRateDialingHours = c.Double(nullable: false),
                        PayRateSuccess = c.Double(nullable: false),
                        BudgetedHours = c.Double(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "dbo.UserProjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ProjectId = c.Int(nullable: false),
                        UserCampaignId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.UserCampaigns", t => t.UserCampaignId)
                .Index(t => t.UserId)
                .Index(t => t.ProjectId)
                .Index(t => t.UserCampaignId);
            
            CreateTable(
                "dbo.UserHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        CampaignId = c.Int(),
                        LoggedIn = c.DateTime(nullable: false),
                        LoggedOff = c.DateTime(nullable: false),
                        Duration = c.Double(),
                        ConnectionId = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "dbo.ZipCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        STD = c.Int(nullable: false),
                        DST = c.Int(nullable: false),
                        TimeZoneId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimeZones", t => t.TimeZoneId)
                .Index(t => t.TimeZoneId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZipCodes", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.UserHistories", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserHistories", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.UserProjects", "UserCampaignId", "dbo.UserCampaigns");
            DropForeignKey("dbo.UserProjects", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserProjects", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.UserCampaigns", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCampaigns", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.StateRestrictions", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OnWatchSettings", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.CampaignCallCodes", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.CampaignCallCodes", "CallCodeId", "dbo.CallCodes");
            DropForeignKey("dbo.AreaCodes", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.Appointments", "RecordId", "dbo.Records");
            DropForeignKey("dbo.Appointments", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.Appointments", "AgentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AgentLogs", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.AgentLogs", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.AgentLogs", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.AgentLogs", "AgentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Records", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Records", "FormId", "dbo.Forms");
            DropForeignKey("dbo.Records", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.Records", "CallCodeId", "dbo.CallCodes");
            DropForeignKey("dbo.CallCodes", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.AnsweredForms", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AnsweredForms", "RecordId", "dbo.Records");
            DropForeignKey("dbo.AnsweredForms", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Contacts", "TimeZoneId", "dbo.TimeZones");
            DropForeignKey("dbo.ExtraFieldValues", "ExtraFieldId", "dbo.ExtraFields");
            DropForeignKey("dbo.ExtraFieldOptions", "ExtraFieldId", "dbo.ExtraFields");
            DropForeignKey("dbo.ExtraFields", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.ProjectPriorityDetails", "ProjectPriorityId", "dbo.ProjectPriorities");
            DropForeignKey("dbo.ProjectPriorities", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Contacts", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.Questions", "FormId", "dbo.Forms");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Forms", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.Campaigns", "ClientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Campaigns", "AccountManagerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ExtraFieldValues", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.Contacts", "AgentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ZipCodes", new[] { "TimeZoneId" });
            DropIndex("dbo.UserHistories", new[] { "CampaignId" });
            DropIndex("dbo.UserHistories", new[] { "UserId" });
            DropIndex("dbo.UserProjects", new[] { "UserCampaignId" });
            DropIndex("dbo.UserProjects", new[] { "ProjectId" });
            DropIndex("dbo.UserProjects", new[] { "UserId" });
            DropIndex("dbo.UserCampaigns", new[] { "CampaignId" });
            DropIndex("dbo.UserCampaigns", new[] { "UserId" });
            DropIndex("dbo.StateRestrictions", new[] { "TimeZoneId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.OnWatchSettings", new[] { "CampaignId" });
            DropIndex("dbo.CampaignCallCodes", new[] { "CallCodeId" });
            DropIndex("dbo.CampaignCallCodes", new[] { "CampaignId" });
            DropIndex("dbo.AreaCodes", new[] { "TimeZoneId" });
            DropIndex("dbo.Appointments", new[] { "AgentId" });
            DropIndex("dbo.Appointments", new[] { "RecordId" });
            DropIndex("dbo.Appointments", new[] { "CampaignId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.CallCodes", new[] { "CampaignId" });
            DropIndex("dbo.AnsweredForms", new[] { "UserId" });
            DropIndex("dbo.AnsweredForms", new[] { "RecordId" });
            DropIndex("dbo.AnsweredForms", new[] { "QuestionId" });
            DropIndex("dbo.Records", new[] { "CallCodeId" });
            DropIndex("dbo.Records", new[] { "FormId" });
            DropIndex("dbo.Records", new[] { "ContactId" });
            DropIndex("dbo.Records", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.ExtraFieldOptions", new[] { "ExtraFieldId" });
            DropIndex("dbo.ProjectPriorityDetails", new[] { "ProjectPriorityId" });
            DropIndex("dbo.ProjectPriorities", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "CampaignId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropIndex("dbo.Questions", new[] { "FormId" });
            DropIndex("dbo.Forms", new[] { "CampaignId" });
            DropIndex("dbo.Campaigns", new[] { "ClientId" });
            DropIndex("dbo.Campaigns", new[] { "AccountManagerId" });
            DropIndex("dbo.ExtraFields", new[] { "CampaignId" });
            DropIndex("dbo.ExtraFieldValues", new[] { "ContactId" });
            DropIndex("dbo.ExtraFieldValues", new[] { "ExtraFieldId" });
            DropIndex("dbo.Contacts", new[] { "ProjectId" });
            DropIndex("dbo.Contacts", new[] { "AgentId" });
            DropIndex("dbo.Contacts", new[] { "TimeZoneId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AgentLogs", new[] { "TimeZoneId" });
            DropIndex("dbo.AgentLogs", new[] { "CampaignId" });
            DropIndex("dbo.AgentLogs", new[] { "ProjectId" });
            DropIndex("dbo.AgentLogs", new[] { "AgentId" });
            DropTable("dbo.ZipCodes");
            DropTable("dbo.UserHistories");
            DropTable("dbo.UserProjects");
            DropTable("dbo.UserCampaigns");
            DropTable("dbo.StateRestrictions");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Penalties");
            DropTable("dbo.OnWatchSettings");
            DropTable("dbo.ExportSettings");
            DropTable("dbo.CampaignCallCodes");
            DropTable("dbo.AreaCodes");
            DropTable("dbo.Appointments");
            DropTable("dbo.AlertSettings");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.CallCodes");
            DropTable("dbo.AnsweredForms");
            DropTable("dbo.Records");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.TimeZones");
            DropTable("dbo.ExtraFieldOptions");
            DropTable("dbo.ProjectPriorityDetails");
            DropTable("dbo.ProjectPriorities");
            DropTable("dbo.Projects");
            DropTable("dbo.Answers");
            DropTable("dbo.Questions");
            DropTable("dbo.Forms");
            DropTable("dbo.Campaigns");
            DropTable("dbo.ExtraFields");
            DropTable("dbo.ExtraFieldValues");
            DropTable("dbo.Contacts");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AgentLogs");
        }
    }
}
