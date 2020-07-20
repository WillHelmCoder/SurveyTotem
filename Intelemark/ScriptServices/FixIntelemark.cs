using Intelemark.Entities;
using Intelemark.Models;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Hosting;

namespace Intelemark.ScriptServices
{
    public class FixIntelemark
    {
        public void Run()
        {
            //TelemetryDebugWriter.IsTracingDisabled = true;
            TelemetryConfiguration.Active.DisableTelemetry = true;

            CreateUsers();
            CreateDomainLevel1();
            CreateDomainLevel2();
            CreateZipCodes();
            CreateContacts();
            CreateDomainLevel3();
            CreateDomainLevel4();
        }

        public void CreateUsers()
        {
            try
            {
                var ctx = ApplicationDbContext.Create();

                //already OK
                if (ctx.Users.Any())
                    return;

                var reader = new ReadOnlyContext();

                var db = MemoryDB.LoadForUsers(reader);

                foreach (var item in db.Roles)
                {
                    var role = new IdentityRole
                    {
                        Id = item.Id,
                        Name = item.Name,
                    };

                    ctx.Roles.Add(role);
                }

                foreach (var item in db.ApplicationUsers)
                {
                    var user = new ApplicationUser
                    {
                        Id = item.Id,

                        AccessFailedCount = item.AccessFailedCount,
                        
                        CreationDate = item.CreationDate,
                        Email = item.Email,
                        EmailConfirmed = item.EmailConfirmed,
                        EndDate = item.EndDate,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        LockoutEnabled = item.LockoutEnabled,
                        LockoutEndDateUtc = item.LockoutEndDateUtc,                       
                        PasswordHash = item.PasswordHash,
                        PhoneNumber = item.PhoneNumber,
                        PhoneNumberConfirmed = item.PhoneNumberConfirmed,
                        SecurityStamp = item.SecurityStamp,
                        StartDate = item.StartDate,                       
                        TwoFactorEnabled = item.TwoFactorEnabled,
                        UserName = item.UserName,
                      
                    };

                    foreach (var role in item.Roles)
                    {
                        user.Roles.Add(new IdentityUserRole
                        {
                            RoleId = role.RoleId,
                            UserId = user.Id,
                        });
                    }

                    ctx.Users.Add(user);
                }

                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var str = ex.ToString();
                System.Diagnostics.Debug.WriteLine(str);
            }
        }
        public void CreateDomainLevel1()
        {
            try
            {
                var ctx = ApplicationDbContext.Create();

                //already OK
                if (ctx.Campaigns.Any())
                    return;

                var reader = new ReadOnlyContext();

                var db = MemoryDB.LoadDomainLevel1(reader);

                var container = new MemoryDB();

                foreach (var item in db.Campaigns)
                {
                    var campaign = new Campaign
                    {
                        Id = item.Id,

                        //AccountManager = accountManager,
                        //AccountManagerId = accountManager?.Id,
                        AccountManagerId = item.AccountManagerId,

                        //Client = client,
                        //ClientId = client?.Id,
                        ClientId = item.ClientId,

                        ActiveControl = item.ActiveControl,
                        BillingPH = item.BillingPH,
                        CampaignLimit = item.CampaignLimit,
                        CompanyLink = item.CompanyLink,
                        CreationDate = item.CreationDate,
                        Description = item.Description,
                        Identifier = item.Identifier,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        MaxAttempt = item.MaxAttempt,
                        Objective = item.Objective,
                        SpellCheck = item.SpellCheck,
                    };

                    container.Campaigns.Add(campaign);
                }

                foreach (var item in db.Projects)
                {
                    var project = new Project
                    {
                        Id = item.Id,

                        //Campaign = campaign,
                        //CampaignId = campaign.Id,
                        CampaignId = item.CampaignId,

                        CreationDate = item.CreationDate,
                        Description = item.Description,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        Name = item.Name,
                        Priority = item.Priority,
                    };

                    container.Projects.Add(project);
                }

                foreach (var item in db.TimeZones)
                {
                    var timeZone = new Entities.TimeZone
                    {
                        Id = item.Id,

                        Code = item.Code,
                        CreationDate = item.CreationDate,
                        CurrentTime = item.CurrentTime,
                        DST = item.DST,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        Name = item.Name,
                        STD = item.STD,
                    };

                    container.TimeZones.Add(timeZone);
                }

                foreach (var item in db.Penalties)
                {
                    var penalty = new Penalty
                    {
                        Id = item.Id,

                        CreationDate = item.CreationDate,
                        From = item.From,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        PayRate = item.PayRate,
                        PenaltyFee = item.PenaltyFee,
                        To = item.To,
                    };

                    container.Penalties.Add(penalty);
                }

                foreach (var item in db.AlertSettings)
                {
                    var alertSettings = new AlertSettings
                    {
                        Id = item.Id,

                        LastUpdate = item.LastUpdate,
                        CreationDate = item.CreationDate,
                        DataPercentage = item.DataPercentage,
                        IsDeleted = item.IsDeleted,
                        NotificationEmails = item.NotificationEmails,
                    };

                    container.AlertSettings.Add(alertSettings);
                }

                foreach (var item in db.AreaCodes)
                {
                    var areaCode = new AreaCode
                    {
                        Id = item.Id,

                        //TimeZone = timeZone,
                        //TimeZoneId = timeZone.Id,
                        TimeZoneId = item.TimeZoneId,

                        CreationDate = item.CreationDate,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        Name = item.Name,
                        TZ = item.TZ,
                    };

                    container.AreaCodes.Add(areaCode);
                }

                foreach (var item in db.CallCodes)
                {
                    var campaign = container.Campaigns
                        .SingleOrDefault(x => x.Id == item.CampaignId);

                    var callCode = new CallCode(
                        campaign,
                        item.Name,
                        item.Code,
                        item.Behavior,
                        item.IsSuccess,
                        item.CreationDate);

                    callCode.IsDeleted = item.IsDeleted;

                    container.CallCodes.Add(callCode);
                }

                foreach (var item in db.ExportSettings)
                {
                    var exportSettings = new ExportSettings
                    {
                        Id = item.Id,

                        //NOT FK
                        CampaignId = item.CampaignId,

                        CreationDate = item.CreationDate,
                        IsDeleted = item.IsDeleted,
                        Key = item.Key,
                        LastUpdate = item.LastUpdate,
                        Name = item.Name,
                        Value = item.Value,
                    };

                    container.ExportSettings.Add(exportSettings);
                }

                foreach (var item in db.ProjectPriorities)
                {
                    var projectPriority = new ProjectPriority
                    {
                        Id = item.Id,

                        //Project = item.Project,
                        //ProjectId = project.Id,
                        ProjectId = item.ProjectId,

                        LastUpdate = item.LastUpdate,
                        IsDeleted = item.IsDeleted,
                        CreationDate = item.CreationDate,
                        Field = item.Field,
                        PriorityValue = item.PriorityValue,
                    };

                    container.ProjectPriorities.Add(projectPriority);
                }

                foreach (var item in db.ProjectPriorityDetails)
                {
                    var projectPriorityDetail = new ProjectPriorityDetail
                    {
                        Id = item.Id,

                        //ProjectPriority = projectPriority,
                        //ProjectPriorityId = projectPriority.Id,
                        ProjectPriorityId = item.ProjectPriorityId,

                        CreationDate = item.CreationDate,
                        FieldPriorityValue = item.FieldPriorityValue,
                        FieldValue = item.FieldValue,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                    };

                    container.ProjectPriorityDetails.Add(projectPriorityDetail);
                }

                foreach (var item in db.StateRestrictions)
                {
                    var stateRestriction = new StateRestriction
                    {
                        Id = item.Id,

                        //TimeZone = timeZone,
                        //TimeZoneId = timeZone.Id,
                        TimeZoneId = item.TimeZoneId,

                        LastUpdate = item.LastUpdate,
                        IsDeleted = item.IsDeleted,
                        Abbreviation = item.Abbreviation,
                        CreationDate = item.CreationDate,
                        IsRestricted = item.IsRestricted,
                        Name = item.Name,
                    };

                    container.StateRestrictions.Add(stateRestriction);
                }

                foreach (var item in db.OnWatchSettings)
                {
                    var onWatchSettings = new OnWatchSettings
                    {
                        Id = item.Id,

                        //Campaign = campaign,
                        //CampaignId = campaign.Id,

                        CampaignId = item.CampaignId,
                        CreationDate = item.CreationDate,
                        HoursLeft = item.HoursLeft,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                    };

                    container.OnWatchSettings.Add(onWatchSettings);
                }

                ctx.Campaigns.AddRange(container.Campaigns);
                ctx.Projects.AddRange(container.Projects);
                ctx.TimeZones.AddRange(container.TimeZones);
                ctx.Penalties.AddRange(container.Penalties);
                ctx.AlertSettings.AddRange(container.AlertSettings);
                ctx.AreaCodes.AddRange(container.AreaCodes);
                ctx.CallCodes.AddRange(container.CallCodes);
                ctx.ExportSettings.AddRange(container.ExportSettings);
                ctx.ProjectPriorities.AddRange(container.ProjectPriorities);
                ctx.ProjectPriorityDetails.AddRange(container.ProjectPriorityDetails);
                ctx.StateRestrictions.AddRange(container.StateRestrictions);

                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var str = ex.ToString();
                System.Diagnostics.Debug.WriteLine(str);
            }
        }
        public void CreateDomainLevel2()
        {
            try
            {
                var ctx = ApplicationDbContext.Create();

                //already OK
                if (ctx.Forms.Any())
                    return;

                var reader = new ReadOnlyContext();

                var db = MemoryDB.LoadDomainLevel2(reader);

                var container = new MemoryDB();

                foreach (var item in db.Forms)
                {
                    var form = new Form
                    {
                        Id = item.Id,

                        //Campaign = campaign,
                        //CampaignId = campaign.Id,
                        CampaignId = item.CampaignId,

                        CreationDate = item.CreationDate,
                        Description = item.Description,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        Name = item.Name,
                    };

                    container.Forms.Add(form);
                }

                foreach (var item in db.AgentLogs)
                {
                    var agentLog = new AgentLog
                    {
                        Id = item.Id,

                        //Agent = agent,
                        //AgentId = agent.Id,
                        AgentId = item.AgentId,

                        //Campaign = campaign,
                        //CampaignId = campaign.Id,
                        CampaignId = item.CampaignId,

                        //TimeZone = timeZone,
                        //TimeZoneId = timeZone.Id,
                        TimeZoneId = item.TimeZoneId,

                        CreationDate = item.CreationDate,
                        DialingHours = item.DialingHours,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        Project = item.Project,
                        ProjectId = item.ProjectId,
                        Successes = item.Successes,
                        TrainingHours = item.TrainingHours,
                    };

                    container.AgentLogs.Add(agentLog);
                }

                //foreach (var item in db.CampaignCallCodes)
                //{
                //    var campaignCallCodes = new CampaignCallCode
                //    {
                //        Id = item.Id,

                //        //CallCode = callCode,
                //        //CallCodeId = callCode.Id,
                //        CallCodeId = item.CallCodeId,

                //        //Campaign = campaign,
                //        //CampaignId = campaign.Id,
                //        CampaignId = item.CampaignId,

                //        CreationDate = item.CreationDate,
                //        IsDeleted = item.IsDeleted,
                //        LastUpdate = item.LastUpdate,
                //    };

                //    container.CampaignCallCodes.Add(campaignCallCodes);
                //}

                foreach (var item in db.ExtraFields)
                {
                    var extraField = new ExtraField
                    {
                        Id = item.Id,

                        //Campaign = campaign,
                        //CampaignId = campaign.Id,
                        CampaignId = item.CampaignId,

                        CreationDate = item.CreationDate,
                     //   ExtraFieldOptions = item.ExtraFieldOptions,
                        FieldName = item.FieldName,
                        IsDeleted = item.IsDeleted,
                        TypeId = item.TypeId,
                        LastUpdate = item.LastUpdate,
                    };

                    container.ExtraFields.Add(extraField);
                }

                foreach (var item in db.UserCampaigns)
                {
                    var userCampaign = new UserCampaign
                    {
                        Id = item.Id,

                        //Campaign = campaign,
                        //CampaignId = campaign.Id,
                        CampaignId = item.CampaignId,

                        //User = user,
                        //UserId = user.Id,
                        UserId = item.UserId,

                        BudgetedHours = item.BudgetedHours,
                        CreationDate = item.CreationDate,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        PayRateDialingHours = item.PayRateDialingHours,
                        PayRateSuccess = item.PayRateSuccess,
                        PayRateTrainingHours = item.PayRateTrainingHours,
                    };

                    container.UserCampaigns.Add(userCampaign);
                }

                foreach (var item in db.UserProjects)
                {
                    var userProject = new UserProject
                    {
                        Id = item.Id,

                        //UserCampaign = userCampaign,
                        //UserCampaignId = userCampaign.Id,
                        UserCampaignId = item.UserCampaignId,

                        //Project = project,
                        //ProjectId = project.Id,
                        ProjectId = item.ProjectId,

                        //User = user,
                        //UserId = user.Id,
                        UserId = item.UserId,

                        CreationDate = item.CreationDate,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                    };

                    container.UserProjects.Add(userProject);
                }

                foreach (var item in db.UsersHistory)
                {
                    var userHistory = new UserHistory
                    {
                        Id = item.Id,

                        //User = user,
                        //UserId = user.Id,
                        UserId = item.UserId,

                        //Campaign = campaign,
                        //CampaignId = campaign?.Id,
                        CampaignId = item.CampaignId,

                        ConnectionId = item.ConnectionId,
                        CreationDate = item.CreationDate,
                        Duration = item.Duration,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        LoggedIn = item.LoggedIn,
                        LoggedOff = item.LoggedOff,
                    };

                    container.UsersHistory.Add(userHistory);
                }

                ctx.Forms.AddRange(container.Forms);
                ctx.AgentLogs.AddRange(container.AgentLogs);
                //ctx.CampaignCallCodes.AddRange(container.CampaignCallCodes);
                ctx.ExtraFields.AddRange(container.ExtraFields);
                ctx.UserCampaigns.AddRange(container.UserCampaigns);
                ctx.UserProjects.AddRange(container.UserProjects);
                ctx.UsersHistory.AddRange(container.UsersHistory);

                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var str = ex.ToString();
                System.Diagnostics.Debug.WriteLine(str);
            }
        }
        public void CreateContacts()
        {
            try
            {
                var ctx = ApplicationDbContext.Create();

                //already OK
                if (ctx.Contacts.Count() >= 430450)
                    return;

                var container = new MemoryDB();

                void Save()
                {
                    using (ctx = ApplicationDbContext.Create())
                    {
                        ctx.Contacts.AddRange(container.Contacts);
                        ctx.SaveChanges();
                    }

                    container.Contacts.Clear();
                }

                var reader = new ReadOnlyContext();

                var db = MemoryDB.LoaContacts(reader);

                db.Contacts = db.Contacts
                    .OrderBy(x => x.Id)
                    .ToList();

                var cdict = ctx.Contacts
                    .OrderBy(x => x.Id)
                    .Select(x => x.Id)
                    .ToDictionary(x => x);

                var step = 2503;
                for (int i = 0; i < db.Contacts.Count; i++)
                {
                    if (container.Contacts.Count >= step)
                        Save();

                    if (cdict.ContainsKey(db.Contacts[i].Id))
                        continue;

                    cdict.Add(db.Contacts[i].Id, db.Contacts[i].Id);
                    container.Contacts.Add(new Contact
                    {
                        Id = db.Contacts[i].Id,

                        //Agent = agent,
                        //AgentId = agent?.Id,
                        AgentId = db.Contacts[i].AgentId,

                        //Project = project,
                        //ProjectId = project.Id,
                        ProjectId = db.Contacts[i].ProjectId,

                        //TimeZone = timeZone,
                        //TimeZoneId = timeZone.Id,
                        TimeZoneId = db.Contacts[i].TimeZoneId,

                        Address = db.Contacts[i].Address,
                        AltAddress = db.Contacts[i].AltAddress,
                        AltPhoneNumber = db.Contacts[i].AltPhoneNumber,
                        Attempts = db.Contacts[i].Attempts,
                        City = db.Contacts[i].City,
                        Company = db.Contacts[i].Company,
                        Country = db.Contacts[i].Country,
                        CreationDate = db.Contacts[i].CreationDate,
                        Email = db.Contacts[i].Email,
                        Extension = db.Contacts[i].Extension,
                        InHold = db.Contacts[i].InHold,
                        IsDeleted = db.Contacts[i].IsDeleted,
                        IsFinalized = db.Contacts[i].IsFinalized,
                        LastUpdate = db.Contacts[i].LastUpdate,
                        Name = db.Contacts[i].Name,
                        Notes = db.Contacts[i].Notes,
                        OnDial = db.Contacts[i].OnDial,
                        PhoneNumber = db.Contacts[i].PhoneNumber,
                        SIC = db.Contacts[i].SIC,
                        State = db.Contacts[i].State,
                        Title = db.Contacts[i].Title,
                        ZipCode = db.Contacts[i].ZipCode,
                    });
                }

                if (container.Contacts.Any())
                    Save();
            }
            catch (Exception ex)
            {
                var str = ex.ToString();
                System.Diagnostics.Debug.WriteLine(str);
            }
        }
        public void CreateZipCodes()
        {
            try
            {
                var ctx = ApplicationDbContext.Create();

                //already OK
                if (ctx.ZipCodes.Any())
                    return;

                var reader = new ReadOnlyContext();

                var db = MemoryDB.LoadZipCodes(reader);

                var container = new MemoryDB();

                foreach (var item in db.ZipCodes)
                {
                    var zipCode = new ZipCode
                    {
                        Id = item.Id,

                        //TimeZone = timeZone,
                        //TimeZoneId = timeZone.Id,
                        TimeZoneId = item.TimeZoneId,

                        CreationDate = item.CreationDate,
                        DST = item.DST,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        Name = item.Name,
                        STD = item.STD,
                    };

                    container.ZipCodes.Add(zipCode);
                }

                ctx.ZipCodes.AddRange(container.ZipCodes);

                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var str = ex.ToString();
                System.Diagnostics.Debug.WriteLine(str);
            }
        }
        public void CreateDomainLevel3()
        {
            try
            {
                var ctx = ApplicationDbContext.Create();

                //already OK
                if (ctx.Questions.Any())
                    return;

                var reader = new ReadOnlyContext();

                var db = MemoryDB.LoadDomainLevel3(reader);

                var container = new MemoryDB();

                foreach (var item in db.Questions)
                {
                    var question = new Question
                    {
                        Id = item.Id,

                        //Form = form,
                        //FormId = form.Id,
                        FormId = item.FormId,

                        TypeId = item.TypeId,
                        CreationDate = item.CreationDate,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        Name = item.Name,
                        Order = item.Order,
                    };

                    container.Questions.Add(question);
                }

                foreach (var item in db.Answers)
                {
                    var answer = new Answer
                    {
                        Id = item.Id,

                        //Question = question,
                        //QuestionId = question.Id,
                        QuestionId = item.QuestionId,

                        CreationDate = item.CreationDate,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        Name = item.Name,
                        Order = item.Order,
                    };

                    container.Answers.Add(answer);
                }

                //foreach (var item in db.ExtraFieldOptions)
                //{
                //    var extraFieldOption = new ExtraFieldOption
                //    {
                //        Id = item.Id,

                //        //ExtraField = extraField,
                //        //ExtraFieldId = extraField.Id,
                //        ExtraFieldId = item.ExtraFieldId,

                //        CreationDate = item.CreationDate,
                //        FieldOptionName = item.FieldOptionName,
                //        IsDeleted = item.IsDeleted,
                //        LastUpdate = item.LastUpdate,
                //    };

                //    container.ExtraFieldOptions.Add(extraFieldOption);
                //}

                ctx.Questions.AddRange(container.Questions);
                ctx.Answers.AddRange(container.Answers);
               // ctx.ExtraFieldOptions.AddRange(container.ExtraFieldOptions);

                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var str = ex.ToString();
                System.Diagnostics.Debug.WriteLine(str);
            }
        }
        public void CreateDomainLevel4()
        {
            try
            {
                var ctx = ApplicationDbContext.Create();

                //already OK
                if (ctx.Records.Any())
                    return;

                var reader = new ReadOnlyContext();

                var db = MemoryDB.LoadDomainLevel4(reader);

                var container = new MemoryDB();

                foreach (var item in db.Records)
                {
                    var record = new Record
                    {
                        Id = item.Id,

                        //Form = form,
                        //FormId = form?.Id,
                        FormId = item.FormId,

                        //User = user,
                        //UserId = user?.Id,
                        UserId = item.UserId,

                        //CallCode = callCode,
                        //CallCodeId = callCode.Id,
                        CallCodeId = item.CallCodeId,

                        //Contact = contact,
                        //ContactId = contact.Id,
                        ContactId = item.ContactId,

                        CreationDate = item.CreationDate,
                        Duration = item.Duration,
                        EndTime = item.EndTime,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                        StartTime = item.StartTime,
                    };

                    container.Records.Add(record);
                }

                foreach (var item in db.AnsweredForms)
                {
                    var answeredForm = new AnsweredForm
                    {
                        Id = item.Id,

                        //Question = question,
                        //QuestionId = question.Id,
                        QuestionId = item.QuestionId,

                        //User = user,
                        //UserId = user.Id,
                    

                        //Record = record,
                        //RecordId = record.Id,
                       

                        Answer = item.Answer,
                        CreationDate = item.CreationDate,
                        IsDeleted = item.IsDeleted,
                        LastUpdate = item.LastUpdate,
                    };

                    container.AnsweredForms.Add(answeredForm);
                }

                foreach (var item in db.Appointments)
                {
                    var appointment = new Appointments
                    {
                        Id = item.Id,

                        //Agent = agent,
                        //AgentId = agent?.Id,
                        AgentId = item.AgentId,

                        //Campaign = campaign,
                        //CampaignId = campaign.Id,
                        CampaignId = item.CampaignId,

                        //Record = record,
                        //RecordId = record?.Id,
                        RecordId = item.RecordId,

                        IsDeleted = item.IsDeleted,
                        CreationDate = item.CreationDate,
                        DateScheduled = item.DateScheduled,
                        IsConfirmed = item.IsConfirmed,
                        IsScheduled = item.IsScheduled,
                        LastUpdate = item.LastUpdate,
                        Notes = item.Notes,
                        Notified = item.Notified,
                        ReminderDate = item.ReminderDate,
                        TimesScheduled = item.TimesScheduled,
                    };

                    container.Appointments.Add(appointment);
                }

                //foreach (var item in db.ExtraFieldValues)
                //{
                //    var extraFieldValue = new ExtraFieldValue
                //    {
                //        Id = item.Id,

                //        //Contact = contact,
                //        //ContactId = contact.Id,
                //        ContactId = item.ContactId,

                //        //ExtraField = extraField,
                //        //ExtraFieldId = extraField.Id,
                //        ExtraFieldId = item.ExtraFieldId,

                //        CreationDate = item.CreationDate,
                //        IsDeleted = item.IsDeleted,
                //        LastUpdate = item.LastUpdate,
                //        Value = item.Value,
                //    };

                //    //container.ExtraFieldValues.Add(extraFieldValue);
                //}

                ctx.Records.AddRange(container.Records);
                ctx.AnsweredForms.AddRange(container.AnsweredForms);
                ctx.Appointments.AddRange(container.Appointments);
               // ctx.ExtraFieldValues.AddRange(container.ExtraFieldValues);

                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var str = ex.ToString();
                System.Diagnostics.Debug.WriteLine(str);
            }
        }
    }
    public class MemoryDB
    {
        public static MemoryDB LoadForUsers(ReadOnlyContext reader)
        {
            var db = new MemoryDB
            {
                Roles = reader.Roles.ToList(),
                ApplicationUsers = reader.Users.Include(x => x.Roles).ToList(),
            };

            return db;
        }
        public static MemoryDB LoadDomainLevel1(ReadOnlyContext reader)
        {
            var db = new MemoryDB
            {
                Campaigns = reader.Campaigns.ToList(),
                Projects = reader.Projects.ToList(),
                TimeZones = reader.TimeZones.ToList(),
                Penalties = reader.Penalties.ToList(),
                AlertSettings = reader.AlertSettings.ToList(),
                AreaCodes = reader.AreaCodes.ToList(),
                CallCodes = reader.CallCodes.ToList(),
                ExportSettings = reader.ExportSettings.ToList(),
                ProjectPriorities = reader.ProjectPriorities.ToList(),
                ProjectPriorityDetails = reader.ProjectPriorityDetails.ToList(),
                StateRestrictions = reader.StateRestrictions.ToList(),
                OnWatchSettings = reader.OnWatchSettings.ToList(),
            };

            return db;
        }
        public static MemoryDB LoadDomainLevel2(ReadOnlyContext reader)
        {
            var db = new MemoryDB
            {
                Forms = reader.Forms.ToList(),
                AgentLogs = reader.AgentLogs.ToList(),
                //CampaignCallCodes = reader.CampaignCallCodes.ToList(),
                ExtraFields = reader.ExtraFields.ToList(),
                UserCampaigns = reader.UserCampaigns.ToList(),
                UserProjects = reader.UserProjects.ToList(),
                UsersHistory = reader.UsersHistory.ToList(),
            };

            return db;
        }
        public static MemoryDB LoadZipCodes(ReadOnlyContext reader)
        {
            var db = new MemoryDB
            {
                ZipCodes = JsonConvert.DeserializeObject<List<ZipCode>>
                (System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/zipcodes.txt")))
            };

            return db;
        }
        public static MemoryDB LoaContacts(ReadOnlyContext reader)
        {
            var db = new MemoryDB
            {
                Contacts = JsonConvert.DeserializeObject<List<Contact>>
                (System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/fex.txt"))),
            };

            return db;
        }
        public static MemoryDB LoadDomainLevel3(ReadOnlyContext reader)
        {
            var db = new MemoryDB
            {
                Questions = reader.Questions.ToList(),
                Answers = reader.Answers.ToList(),
                //ExtraFieldOptions = reader.ExtraFieldOptions.ToList(),
            };

            return db;
        }
        public static MemoryDB LoadDomainLevel4(ReadOnlyContext reader)
        {
            var db = new MemoryDB
            {
                Records = reader.Records.ToList(),
                AnsweredForms = reader.AnsweredForms.ToList(),
                Appointments = reader.Appointments.ToList(),

               // ExtraFieldValues = JsonConvert.DeserializeObject<List<ExtraFieldValue>>
                //(System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/efv.txt")))
            };

            return db;
        }

        #region Lists

        public List<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
        public List<Campaign> Campaigns { get; set; } = new List<Campaign>();
        public List<Form> Forms { get; set; } = new List<Form>();
        public List<Question> Questions { get; set; } = new List<Question>();
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public List<StateRestriction> StateRestrictions { get; set; } = new List<StateRestriction>();
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        public List<Project> Projects { get; set; } = new List<Project>();
        public List<Entities.TimeZone> TimeZones { get; set; } = new List<Entities.TimeZone>();
        public List<ZipCode> ZipCodes { get; set; } = new List<ZipCode>();
        public List<AreaCode> AreaCodes { get; set; } = new List<AreaCode>();
        public List<UserHistory> UsersHistory { get; set; } = new List<UserHistory>();
        public List<Record> Records { get; set; } = new List<Record>();
        public List<CallCode> CallCodes { get; set; } = new List<CallCode>();
        public List<AnsweredForm> AnsweredForms { get; set; } = new List<AnsweredForm>();
        public List<UserCampaign> UserCampaigns { get; set; } = new List<UserCampaign>();
        public List<UserProject> UserProjects { get; set; } = new List<UserProject>();
        public List<Appointments> Appointments { get; set; } = new List<Appointments>();
        public List<Penalty> Penalties { get; set; } = new List<Penalty>();
        public List<ProjectPriority> ProjectPriorities { get; set; } = new List<ProjectPriority>();
        public List<ProjectPriorityDetail> ProjectPriorityDetails { get; set; } = new List<ProjectPriorityDetail>();
        public List<AgentLog> AgentLogs { get; set; } = new List<AgentLog>();
        public List<AlertSettings> AlertSettings { get; set; } = new List<AlertSettings>();
        public List<OnWatchSettings> OnWatchSettings { get; set; } = new List<OnWatchSettings>();
        public List<ExtraField> ExtraFields { get; set; } = new List<ExtraField>();
     //   public List<ExtraFieldOption> ExtraFieldOptions { get; set; } = new List<ExtraFieldOption>();
       // public List<ExtraFieldValue> ExtraFieldValues { get; set; } = new List<ExtraFieldValue>();
        //public List<CampaignCallCode> CampaignCallCodes { get; set; } = new List<CampaignCallCode>();
        public List<ExportSettings> ExportSettings { get; set; } = new List<ExportSettings>();
        public List<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();

        #endregion
    }
}

/*
public void GetFile()
{
    //var path = HostingEnvironment.MapPath("~/fex.txt");
    //var path = HostingEnvironment.MapPath("~/efv.txt");
    var path = HostingEnvironment.MapPath("~/zipcodes.txt");

    var reader = new ReadOnlyContext();

    //var data = reader.Contacts.ToList();
    //var data = reader.ExtraFieldValues.ToList();
    var data = reader.ZipCodes.ToList();

    var bigDataLOL = JsonConvert.SerializeObject(data);

    System.IO.File.WriteAllText(path, bigDataLOL, System.Text.Encoding.UTF8);

    var breakpoint = true;
}
 */
