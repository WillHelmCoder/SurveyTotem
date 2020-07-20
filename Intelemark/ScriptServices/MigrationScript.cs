using Intelemark.BigMigration;
using Intelemark.Entities;
using Intelemark.Models;
using Intelemark.Utilities;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Intelemark.ScriptServices
{
    public class MigrationScript
    {
        IntelemarkOldModel OldContext;
        ApplicationDbContext Context;
        string PlaceHolderAgentEmail = "placeholder@intelemark.com";
        int Counter = -1;

        public async Task<bool> Run()
        {
            TelemetryConfiguration.Active.DisableTelemetry = true;

            Context = new ApplicationDbContext();
            Context.Configuration.LazyLoadingEnabled = false;
            Context.Configuration.ProxyCreationEnabled = false;

            OldContext = new IntelemarkOldModel();
            OldContext.Configuration.AutoDetectChangesEnabled = false;
            OldContext.Configuration.LazyLoadingEnabled = false;
            OldContext.Configuration.ProxyCreationEnabled = false;

            try
            {
                //await Supervisors();
                //await Clients();
                //await Agents();
                //await Campaigns();
                //await Projects();
                //await Contacts();
                //await FormsQuestionsAnswers();
                //await CallCodes();
                //await AnsweredForms();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        async Task<bool> Supervisors()
        {
            var managerRole = await Context.Roles
                .SingleOrDefaultAsync(x => x.Name == "Account Manager");

            var clientRole = await Context.Roles
                .SingleOrDefaultAsync(x => x.Name == "Client");

            var clients = await Context.Users
                .Where(x => x.Roles.Any(r => r.RoleId == clientRole.Id))
                .ToListAsync();

            var accountManagers = await Context.Users
                .Where(x => x.Roles.Any(r => r.RoleId == managerRole.Id))
                .ToListAsync();

            var oldCampaigns = await OldContext.etCampaigns
                .GroupBy(x => x.CampaignSupervisor)
                .ToListAsync();

            var newUsers = new List<ApplicationUser>();
            foreach (var oldCampaign in oldCampaigns)
            {
                var name = oldCampaign.Key;

                if (string.IsNullOrWhiteSpace(name))
                    continue;

                var accountManagerCounter = accountManagers
                    .Count(x => x.Name == name);

                if (accountManagerCounter > 1)
                {

                }

                var accountManager = accountManagers
                    .SingleOrDefault(x => x.Name == name);

                if (accountManager != null)
                    continue;

                var email = GetTempEmailFromAgentId(name);

                newUsers.Add(new ApplicationUser
                {
                    Name = name,
                    UserName = email,
                    Email = email,
                    CreationDate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                });
            }

            var owinContext = HttpContext.Current.GetOwinContext();
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();

            foreach (var newUser in newUsers)
            {
                var result = await userManager.CreateAsync(newUser, "Test123!");
                if (!result.Succeeded)
                {

                    continue;
                }

                result = await userManager.AddToRoleAsync(newUser.Id, "Account Manager");

                if (!result.Succeeded)
                {

                    continue;
                }
            }

            return true;
        }
        async Task<bool> Clients()
        {
            var users = await Context.Users
                .ToListAsync();

            var oldCampaigns = await OldContext.etCampaigns
                .GroupBy(x => x.Client_Number)
                .ToListAsync();

            var newUsers = new List<ApplicationUser>();

            foreach (var oldCampaign in oldCampaigns)
            {
                var name = oldCampaign.Key;

                if (string.IsNullOrWhiteSpace(name))
                    continue;

                var client = users
                    .SingleOrDefault(x => x.Name == name);

                if (client != null)
                    continue;

                var email = GetTempEmailFromAgentId(name);

                newUsers.Add(new ApplicationUser
                {
                    Name = name,
                    UserName = email,
                    Email = email,
                    CreationDate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                });
            }

            var owinContext = HttpContext.Current.GetOwinContext();
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();

            foreach (var newUser in newUsers)
            {
                var result = await userManager.CreateAsync(newUser, "Test123!");
                if (!result.Succeeded)
                {

                    continue;
                }

                result = await userManager.AddToRoleAsync(newUser.Id, "Client");

                if (!result.Succeeded)
                {

                    continue;
                }
            }

            return true;
        }
        async Task<bool> Agents()
        {
            var agentRole = await Context.Roles
                .SingleOrDefaultAsync(x => x.Name == "Agent");

            var users = await Context.Users
                .Where(x => x.Roles.Any(r => r.RoleId == agentRole.Id))
                .ToListAsync();

            var oldAgents = await OldContext.etAgentMasters
                .Where(x => x.SecurityLevel == "Agent" || x.SecurityLevel == "QC")
                .ToListAsync();

            var newUsers = new List<ApplicationUser>();
            foreach (var oldAgent in oldAgents)
            {
                var email = GetTempEmailFromAgentId(oldAgent.AgentID);

                var agent = users
                    .SingleOrDefault(x => x.Email == email);

                if (agent != null)
                    continue;

                newUsers.Add(new ApplicationUser
                {
                    Name = oldAgent.AgentName,
                    UserName = email,
                    Email = email,
                    Address = oldAgent.Address,
                    ZipCode = oldAgent.Zip,
                    PhoneNumber = oldAgent.Phone,
                    City = oldAgent.City,
                    State = oldAgent.State,
                    Country = null,
                    StartDate = oldAgent.AgentStartDate,
                    EndDate = oldAgent.AgentStopDate,

                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                    CreationDate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                });
            }

            var owinContext = HttpContext.Current.GetOwinContext();
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();

            foreach (var newUser in newUsers)
            {
                var result = await userManager.CreateAsync(newUser, "Test123!");
                if (!result.Succeeded)
                    continue;

                result = await userManager.AddToRoleAsync(newUser.Id, "Agent");

                if (!result.Succeeded)
                    continue;
            }

            return true;
        }
        async Task<bool> Campaigns()
        {
            var campaigns = await Context.Campaigns
                .ToListAsync();

            var users = await Context.Users
                .ToListAsync();

            var managerRole = await Context.Roles
                .SingleOrDefaultAsync(x => x.Name == "Account Manager");

            var clientRole = await Context.Roles
                .SingleOrDefaultAsync(x => x.Name == "Client");

            var clients = await Context.Users
                .Where(x => x.Roles.Any(r => r.RoleId == clientRole.Id))
                .ToListAsync();

            var accountManagers = await Context.Users
                .Where(x => x.Roles.Any(r => r.RoleId == managerRole.Id))
                .ToListAsync();

            var oldCampaigns = await OldContext.etCampaigns
                .ToListAsync();

            var newCampaigns = new List<Campaign>();
            foreach (var old in oldCampaigns)
            {
                Counter++;

                if (campaigns.Any(x => x.Identifier == old.CampaignId))
                    continue;

                ApplicationUser accountManager = null;
                ApplicationUser client = null;

                if (!string.IsNullOrWhiteSpace(old.CampaignSupervisor))
                {
                    var managers = accountManagers
                        .Count(x => x.Name == old.CampaignSupervisor);

                    if (managers > 1)
                    {

                    }

                    accountManager = accountManagers
                        .SingleOrDefault(x => x.Name == old.CampaignSupervisor);
                }

                if (!string.IsNullOrWhiteSpace(old.Client_Number))
                {
                    var clientsCounter = clients
                        .Count(x => x.Name == old.Client_Number);

                    if (clientsCounter > 1)
                    {

                    }

                    client = clients
                        .SingleOrDefault(x => x.Name == old.Client_Number);
                }

                int.TryParse(old.MaxAttempts, out var maxAttempt);

                newCampaigns.Add(new Campaign
                {
                    AccountManager = accountManager,
                    AccountManagerId = accountManager?.Id,

                    Client = client,
                    ClientId = client?.Id,

                    ActiveControl = old.IsActive?.Trim() == "Y",
                    CompanyLink = old.CompanyLink?.Trim() == "Y",
                    BillingPH = old.BillingPerHourProduction ?? 0,
                    Description = old.CampaignDesc,
                    Identifier = old.CampaignId,
                    Objective = old.CampaignObjective,
                    MaxAttempt = maxAttempt,
                    CreationDate = old.CreatedDate ?? DateTime.Now,
                    LastUpdate = old.CreatedDate ?? DateTime.Now,

                    CampaignLimit = 0,
                    IsDeleted = false,
                    SpellCheck = false
                });
            }

            Context.Campaigns.AddRange(newCampaigns);
            await Context.SaveChangesAsync();

            return true;
        }
        async Task<bool> Projects()
        {
            var grouppings = await OldContext.elMasters
                .GroupBy(x => new { x.CampaignId, x.ProjectId })
                .Select(x => new { x.Key.CampaignId, x.Key.ProjectId })
                .ToListAsync();

            var campaigns = await Context.Campaigns
                .Include(x => x.Projects)
                .ToListAsync();

            var newProjects = new List<Project>();
            foreach (var groupping in grouppings)
            {
                if (string.IsNullOrWhiteSpace(groupping.CampaignId))
                    continue;

                if (string.IsNullOrWhiteSpace(groupping.ProjectId))
                    continue;

                if (groupping.CampaignId == "GGL15b" || groupping.CampaignId == "GGL15B")
                {

                }

                var campaign = campaigns
                    .SingleOrDefault(x => x.Identifier
                    .Equals(groupping.CampaignId, StringComparison.OrdinalIgnoreCase));

                if (campaign == null)
                    continue;

                var project = campaign.Projects
                    .SingleOrDefault(x => x.Name.Equals(groupping.ProjectId));

                if (project == null)
                {
                    newProjects.Add(new Project
                    {
                        Name = groupping.ProjectId,
                        Description = "Migrated",
                        CreationDate = DateTime.Now,
                        LastUpdate = DateTime.Now,
                        Priority = Utilities.Priorities.High,

                        Campaign = campaign,
                        CampaignId = campaign.Id,

                        IsDeleted = false,
                        ProjectPriority = new List<ProjectPriority>(),
                    });
                }
            }

            Context.Projects.AddRange(newProjects);
            await Context.SaveChangesAsync();

            return true;
        }
        async Task<bool> Contacts()
        {
            var noneTimeZone = await Context.TimeZones
                .SingleOrDefaultAsync(x => x.Code == "NONE");

            if (noneTimeZone == null)
            {
                Context.TimeZones.Add(new Entities.TimeZone
                {
                    Name = "None",
                    Code = "NONE",
                    CreationDate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                });

                await Context.SaveChangesAsync();
            }

            var newGroups = await Context.Projects
                .GroupBy(x => new NewGroup
                {
                    CampaignId = x.CampaignId,
                    Identifier = x.Campaign.Identifier,

                    ProjectId = x.Id,
                    Name = x.Name,
                    Contacts = x.Contacts.Count()
                })
                .Select(x => x.Key)
                .ToListAsync();

            var oldItems = await OldContext.elMasters
                .GroupBy(x => new
                {
                    x.CampaignId,
                    x.ProjectId
                })
                .Select(x => new OldGroup
                {
                    CampaignId = x.Key.CampaignId,
                    ProjectId = x.Key.ProjectId,
                    Contacts = x.Count()
                })
                .OrderBy(x => x.Contacts)
                .ToListAsync();

            var groups = new List<NewGroup>();
            foreach (var item in oldItems)
            {
                var newGroup = newGroups
                    .SingleOrDefault(x =>
                        x.Identifier.Equals(item.CampaignId, StringComparison.OrdinalIgnoreCase)
                        && x.Name.Equals(item.ProjectId, StringComparison.OrdinalIgnoreCase));

                if (newGroup == null)
                    continue;

                groups.Add(newGroup);
            }

            var diff = oldItems.Count - groups.Count;
            foreach (var group in groups)
            {
                if (group.Contacts > 0)
                    continue;

                using (var newContext = new ApplicationDbContext())
                using (var oldContext = new IntelemarkOldModel())
                {
                    newContext.Configuration.LazyLoadingEnabled = false;
                    newContext.Configuration.ProxyCreationEnabled = false;

                    var timeZones = await newContext.TimeZones
                        .ToListAsync();

                    var users = await newContext.Users
                        .ToListAsync();

                    var project = await newContext.Projects
                        .Include(x => x.Campaign)
                        .SingleOrDefaultAsync(x => x.CampaignId == group.CampaignId
                            && x.Id == group.ProjectId);

                    var oldContacts = await oldContext.elMasters
                        .Where(x =>
                            x.CampaignId.Equals(project.Campaign.Identifier, StringComparison.OrdinalIgnoreCase)
                            && x.ProjectId.Equals(project.Name, StringComparison.OrdinalIgnoreCase))
                        .ToListAsync();

                    var newContacts = new List<Contact>();
                    foreach (var oldContact in oldContacts)
                    {
                        int.TryParse(oldContact.PhoneExt, out var phoneExt);

                        ApplicationUser agent = null;
                        if (!string.IsNullOrWhiteSpace(oldContact.AgentId))
                        {
                            agent = users
                                .SingleOrDefault(x => x.Name == oldContact.AgentId);
                        }

                        var timeZone = GetTimeZoneByCode(oldContact.TimeZone, timeZones);

                        newContacts.Add(new Contact
                        {
                            LeadId = oldContact.LeadID,
                            Name = oldContact.Contact,
                            Email = oldContact.Email,
                            Title = oldContact.Title,
                            Company = oldContact.Company,
                            SIC = oldContact.SIC,

                            Address = oldContact.Address1,
                            AltAddress = oldContact.Address2,
                            PhoneNumber = oldContact.Phone1,
                            AltPhoneNumber = oldContact.Mobile,
                            Extension = phoneExt,

                            City = oldContact.City,
                            Country = oldContact.Country,
                            State = oldContact.State,
                            ZipCode = oldContact.Zip,

                            Attempts = oldContact.Attempts ?? 0,
                            Notes = oldContact.Notes,
                            InHold = false,
                            OnDial = false,
                            IsFinalized = oldContact.Status.ToLower() == "x",

                            CreationDate = oldContact.CreatedDate ?? DateTime.Now,
                            LastUpdate = oldContact.CreatedDate ?? DateTime.Now,
                            IsDeleted = false,

                            ProjectId = project.Id,
                            Project = project,

                            AgentId = agent?.Id,
                            Agent = agent,

                            TimeZoneId = timeZone.Id,
                            TimeZoneString = timeZone.Code,
                            TimeZone = timeZone,
                        });
                    }

                    Debug.WriteLine("-----------------------------------");

                    var start = DateTime.Now;
                    newContext.Contacts.AddRange(newContacts);
                    await newContext.SaveChangesAsync();

                    Debug.WriteLine($"" +
                        $"{project.Campaign.Identifier} | " +
                        $"project: {project.Name} | " +
                        $"contacts: {newContacts.Count} | " +
                        $"time: {(DateTime.Now - start).TotalSeconds:0.00} seconds");
                }
            }

            return true;
        }
        async Task<bool> FormsQuestionsAnswers()
        {
            var data = await Context.Campaigns
                .Select(x => new
                {
                    CampaignId = x.Id,
                    x.Identifier
                })
                .ToListAsync();

            foreach (var item in data)
            {
                var hasUDF = await OldContext.eUDFs
                    .AnyAsync(x => x.CampaignId == item.Identifier);

                if (!hasUDF)
                    continue;

                var hasForms = await Context.Forms
                    .AnyAsync(x => x.CampaignId == item.CampaignId);

                if (hasForms)
                    continue;

                using (var newContext = new ApplicationDbContext())
                using (var oldContext = new IntelemarkOldModel())
                {
                    newContext.Configuration.LazyLoadingEnabled = false;
                    newContext.Configuration.ProxyCreationEnabled = false;
                    oldContext.Configuration.LazyLoadingEnabled = false;
                    oldContext.Configuration.ProxyCreationEnabled = false;

                    var eUDF = await oldContext.eUDFs
                        .Where(x => x.CampaignId == item.Identifier)
                        .ToListAsync();

                    var eMoreUDF = await oldContext.eMoreUDFs
                        .Where(x => x.CampaignId == item.Identifier)
                        .ToListAsync();

                    var campaign = await newContext.Campaigns
                        .SingleOrDefaultAsync(x => x.Id == item.CampaignId);

                    var now = DateTime.Now;

                    var form = new Form
                    {
                        CampaignId = campaign.Id,
                        Campaign = campaign,

                        Description = "Migrated",
                        Name = $"{campaign.Identifier}",
                        CreationDate = now,
                        LastUpdate = now,
                        Questions = new List<Question>(),
                    };

                    newContext.Forms.Add(form);

                    QuestionTypes getQuestionType(string text)
                    {
                        switch (text.ToLower())
                        {
                            case "text":
                            case "displayonly": return QuestionTypes.OpenAnswer;

                            case "table": return QuestionTypes.SelectableAnswer;

                            case "datetime": return QuestionTypes.CalendarTimeAnswer;
                            case "date": return QuestionTypes.CalendarAnswer;
                            case "time": return QuestionTypes.TimeAnswer;
                            default: throw new NotImplementedException();
                        }
                    }

                    var newQuestions = new List<Question>();

                    foreach (var udf in eUDF)
                    {
                        if (udf.Format.ToLower() == "hidden")
                            continue;

                        int.TryParse(udf.UDFNum.ToLower().Replace("udf", string.Empty), out var order);
                        if (order == 0)
                        {

                        }

                        var question = new Question
                        {
                            FormId = form.Id,
                            Form = form,

                            Name = udf.Label,
                            Order = order,
                            TypeId = getQuestionType(udf.Format),

                            CreationDate = now,
                            LastUpdate = now,
                            Answers = new List<Answer>(),
                        };

                        newQuestions.Add(question);
                    }

                    foreach (var udf in eMoreUDF)
                    {
                        if (udf.Format.ToLower() == "hidden")
                            continue;

                        int.TryParse(udf.UDFNum.ToLower().Replace("udf", string.Empty), out var order);
                        if (order == 0)
                        {

                        }

                        var question = new Question
                        {
                            FormId = form.Id,
                            Form = form,

                            Name = udf.Label,
                            Order = order,
                            TypeId = getQuestionType(udf.Format),

                            CreationDate = now,
                            LastUpdate = now,
                            Answers = new List<Answer>(),
                        };

                        newQuestions.Add(question);
                    }

                    //form.Questions.Add(newQuestion); ????
                    newContext.Questions.AddRange(newQuestions.OrderBy(x => x.Order));
                    await newContext.SaveChangesAsync();

                    if (!form.Questions.Any(x => x.TypeId == QuestionTypes.SelectableAnswer))
                        continue;

                    //question.Answers.Add(newAnswer); ????
                    var newAnswers = new List<Answer>();

                    var etLookUps = await oldContext.etLookups
                        .Where(x => x.CampaignId == campaign.Identifier)
                        .ToListAsync();

                    foreach (var question in form.Questions)
                    {
                        if (question.TypeId != QuestionTypes.SelectableAnswer)
                            continue;

                        var maskString = string.Empty;
                        if (question.Order <= 16)
                        {
                            var udf = eUDF
                                .SingleOrDefault(x => x.UDFNum.ToLower()
                                .Equals($"udf{question.Order}", StringComparison.OrdinalIgnoreCase));

                            if (udf == null)
                            {

                            }

                            maskString = udf.Mask;
                        }
                        else
                        {
                            var udf = eMoreUDF
                                .SingleOrDefault(x => x.UDFNum.ToLower()
                                .Equals($"udf{question.Order}", StringComparison.OrdinalIgnoreCase));

                            if (udf == null)
                            {

                            }

                            maskString = udf.Mask;
                        }

                        var lookUps = etLookUps
                            .Where(x => x.Tablename == maskString)
                            .ToList();

                        if (!lookUps.Any())
                        {
                            Debug.WriteLine($"lookUp not found: {maskString}");
                            continue;
                        }

                        foreach (var lookUp in lookUps)
                        {
                            newAnswers.Add(new Answer
                            {
                                Name = lookUp.TValue,

                                CreationDate = now,
                                LastUpdate = now,
                                Order = question.Order,

                                Question = question,
                                QuestionId = question.Id,
                            });
                        }

                        if (!newAnswers.Any())
                        {
                            Debug.WriteLine($"Form has SelectableAnswers but no Lookups: {item.CampaignId}");
                            continue;
                        }

                        newContext.Answers.AddRange(newAnswers);
                    }

                    Debug.WriteLine($"" +
                        $"{item.CampaignId} " +
                        $"Q: {form.Questions.Count}" +
                        $"A: {form.Questions.Sum(x => x.Answers.Count)}");

                    await newContext.SaveChangesAsync();
                }
            }

            return true;
        }
        async Task<bool> CallCodes()
        {
            var hasCallCodes = await Context.CallCodes
                .AnyAsync();

            if (hasCallCodes)
                return true;

            using (var newContext = new ApplicationDbContext())
            using (var oldContext = new IntelemarkOldModel())
            {
                newContext.Configuration.LazyLoadingEnabled = false;
                newContext.Configuration.ProxyCreationEnabled = false;
                oldContext.Configuration.LazyLoadingEnabled = false;
                oldContext.Configuration.ProxyCreationEnabled = false;

                var campaigns = await newContext.Campaigns
                    .ToListAsync();

                var callCodes = await newContext.CallCodes
                    .ToListAsync();

                var etEOCList = await oldContext.etEOCs
                    .ToListAsync();

                var now = DateTime.Now;

                EOCBehaviors getBehaviour(etEOC eoc)
                {
                    switch (eoc.Actions.Trim().ToLower())
                    {
                        case "f":
                        case "d": return EOCBehaviors.FinalizeRecord;
                        case "c": return EOCBehaviors.CallBack;
                        case "r": return EOCBehaviors.SetUpAppointment;
                        case "n": return EOCBehaviors.None;

                        default: throw new NotImplementedException();
                    }
                }

                var newCallCodes = new List<CallCode>();
                foreach (var campaign in campaigns)
                {
                    var campaignEOCList = etEOCList
                        .Where(x => x.CampaignId == campaign.Identifier)
                        .ToList();

                    foreach (var campaignEOC in campaignEOCList)
                    {
                        var callCode = new CallCode(
                            campaign,
                            campaignEOC.EOCDesc,
                            campaignEOC.EOC,
                            getBehaviour(campaignEOC),
                            campaignEOC.InSPH.Trim() == "Y",
                            now);

                        newCallCodes.Add(callCode);
                    }
                }

                Debug.WriteLine($"Migrated: {newCallCodes.Count} call codes");

                newContext.CallCodes.AddRange(newCallCodes);

                await newContext.SaveChangesAsync();
            }

            return true;
        }
        async Task<bool> AnsweredForms()
        {
            var data = await OldContext.etCallHistories
                .GroupBy(x => x.CampaignId)
                .Select(x => new
                {
                    Identifier = x.Key,
                    Total = x.Count()
                })
                .OrderBy(x => x.Total)
                .ToListAsync();

            if (!await TryCreatePlaceHolderAgent())
                return false;

            foreach (var item in data)
            {
                var hasUDF = await OldContext.eUDFs
                    .AnyAsync(x => x.CampaignId == item.Identifier);

                if (!hasUDF)
                    continue;

                var hasCalls = await OldContext.etCallHistories
                    .AnyAsync(x => x.CampaignId == item.Identifier);

                if (!hasCalls)
                    continue;

                var campaignId = await Context.Campaigns
                    .Where(x => x.Identifier == item.Identifier)
                    .Select(x => x.Id)
                    .SingleAsync();

                var hasForms = await Context.Forms
                    .AnyAsync(x => x.CampaignId == campaignId);

                if (!hasForms)
                    continue;

                var hasRecords = await Context.Records
                    .AnyAsync(x => x.Form.CampaignId == campaignId);

                if (hasRecords)
                    continue;

                using (var newContext = new ApplicationDbContext())
                using (var oldContext = new IntelemarkOldModel())
                {
                    newContext.Configuration.LazyLoadingEnabled = false;
                    newContext.Configuration.ProxyCreationEnabled = false;
                    oldContext.Configuration.LazyLoadingEnabled = false;
                    oldContext.Configuration.ProxyCreationEnabled = false;

                    //load NEW data
                    var campaign = await newContext.Campaigns
                        .Include(x => x.Projects)
                        .SingleOrDefaultAsync(x => x.Id == campaignId);

                    var projects = campaign.Projects
                        .ToDictionary(x => x.Name.Trim().ToLower());

                    var form = await newContext.Forms
                        .SingleOrDefaultAsync(x => x.CampaignId == campaign.Id);

                    if (form == null)
                        continue;

                    var timeZones = await newContext.TimeZones
                        .ToListAsync();

                    var noneTimeZone = timeZones
                        .Single(x => x.Code == "NONE");

                    var callCodes = await newContext.CallCodes
                        .Where(x => x.CampaignId == campaign.Id)
                        .ToListAsync();

                    var callCodesDictionary = callCodes
                        .ToDictionary(x => x.Code);

                    await newContext.Questions
                        .Include(x => x.Answers)
                        .Where(x => x.FormId == form.Id)
                        .LoadAsync();

                    var contacts = await newContext.Contacts
                        .Where(x => x.Project.CampaignId == campaign.Id)
                        .ToListAsync();

                    var contactsDictionary = contacts
                        .ToDictionary(x => x.LeadId);

                    var agentRole = await newContext.Roles
                        .SingleOrDefaultAsync(x => x.Name == "Agent");

                    var agents = await newContext.Users
                        .Where(x => x.Roles.Any(r => r.RoleId == agentRole.Id))
                        .ToListAsync();

                    var agentDictionary = agents
                        .ToDictionary(x => x.Email);

                    var placeHolderAgent = await newContext.Users
                        .SingleOrDefaultAsync(x => x.Email == PlaceHolderAgentEmail);

                    var orderedQuestions = form.Questions
                        .OrderBy(x => x.Order)
                        .ToList();



                    //load OLD data
                    var eUDF = await oldContext.eUDFs
                        .Where(x => x.CampaignId == item.Identifier)
                        .ToListAsync();

                    var eMoreUDF = await oldContext.eMoreUDFs
                        .Where(x => x.CampaignId == item.Identifier)
                        .ToListAsync();

                    var oldContacts = await oldContext.elMasters
                        .Where(x => x.CampaignId == campaign.Identifier)
                        .ToListAsync();

                    var oldContactsDictionary = oldContacts
                        .ToDictionary(x => x.LeadID);

                    var eMoresDictionary = new Dictionary<int, List<eUDFMore>>();
                    var eMores = await oldContext.eUDFMores
                        .Where(x => x.CampaignId == campaign.Identifier)
                        .ToListAsync();

                    foreach (var eMore in eMores)
                    {
                        if (!eMoresDictionary.ContainsKey(eMore.LeadId))
                            eMoresDictionary[eMore.LeadId] = new List<eUDFMore>();

                        eMoresDictionary[eMore.LeadId].Add(eMore);
                    }

                    var callHistories = await oldContext.etCallHistories
                        .Where(x => x.CampaignId == campaign.Identifier)
                        .ToListAsync();

                    var callsDictionary = new Dictionary<int, List<etCallHistory>>();
                    foreach (var callHistory in callHistories)
                    {
                        if (!callsDictionary.ContainsKey(callHistory.LeadID))
                            callsDictionary[callHistory.LeadID] = new List<etCallHistory>();

                        callsDictionary[callHistory.LeadID].Add(callHistory);
                    }

                    var etEOCList = await oldContext.etEOCs
                        .Where(x => x.CampaignId == campaign.Identifier)
                        .ToListAsync();

                    var now = DateTime.Now;
                    string GetMasterAnswer(elMaster master, int order)
                    {
                        if (master == null)
                            return null;

                        switch (order)
                        {
                            case 1: return master.UDF1;
                            case 2: return master.UDF2;
                            case 3: return master.UDF3;
                            case 4: return master.UDF4;
                            case 5: return master.UDF5;
                            case 6: return master.UDF6;
                            case 7: return master.UDF7;
                            case 8: return master.UDF8;
                            case 9: return master.UDF9;
                            case 10: return master.UDF10;
                            case 11: return master.UDF11;
                            case 12: return master.UDF12;
                            case 13: return master.UDF13;
                            case 14: return master.UDF14;
                            case 15: return master.UDF15;
                            case 16: return master.UDF16;
                        }

                        throw new NotImplementedException();
                    }
                    string GetMoreAnswer(eUDFMore more, int order)
                    {
                        if (more == null)
                            return null;

                        switch (order)
                        {
                            case 17: return more.UDF17;
                            case 18: return more.UDF18;
                            case 19: return more.UDF19;
                            case 20: return more.UDF20;
                            case 21: return more.UDF21;
                            case 22: return more.UDF22;
                            case 23: return more.UDF23;
                            case 24: return more.UDF24;
                            case 25: return more.UDF25;
                            case 26: return more.UDF26;
                            case 27: return more.UDF27;
                            case 28: return more.UDF28;
                            case 29: return more.UDF29;
                            case 30: return more.UDF30;
                            case 31: return more.UDF31;
                            case 32: return more.UDF32;
                        }

                        throw new NotImplementedException();
                    }

                    var newCallCodes = new List<CallCode>();
                    var newRecords = new List<Record>();
                    var newAnsweredForms = new List<AnsweredForm>();
                    var newContacts = new List<Contact>();
                    var notFoundAgents = new List<string>();


                    foreach (var call in callHistories)
                    {
                        if (!projects.ContainsKey(call.ProjectId.Trim().ToLower()))
                            continue;

                        var project = projects[call.ProjectId.Trim().ToLower()];

                        ApplicationUser agent = null;
                        if (!string.IsNullOrWhiteSpace(call.AgentID))
                        {
                            if (agentDictionary.ContainsKey(GetTempEmailFromAgentId(call.AgentID)))
                            {
                                agent = agentDictionary[GetTempEmailFromAgentId(call.AgentID)];
                            }
                            else
                            {
                                notFoundAgents.Add(call.AgentID);
                                agent = placeHolderAgent;
                            }
                        }
                        else
                        {

                        }

                        if (agent == null)
                        {

                        }

                        Contact contact = null;
                        if (!contactsDictionary.ContainsKey(call.LeadID))
                        {
                            var newContact = GetPlaceholderContact(
                                callsDictionary.ContainsKey(call.LeadID) ?
                                callsDictionary[call.LeadID].Count : 1,
                                call.LeadID,
                                project,
                                noneTimeZone,
                                agent);

                            newContacts.Add(newContact);
                            contactsDictionary[newContact.LeadId] = newContact;
                        }

                        contact = contactsDictionary[call.LeadID];

                        elMaster oldContact = null;
                        if (oldContactsDictionary.ContainsKey(call.LeadID))
                            oldContact = oldContactsDictionary[call.LeadID];
                        else
                        {

                        }

                        CallCode callCode = null;

                        var eoc = string.IsNullOrWhiteSpace(call.EOC) ?
                            "NONE" : call.EOC.Trim();

                        if (!callCodesDictionary.ContainsKey(eoc))
                        {
                            callCode = new CallCode(
                                campaign,
                                "Migrated",
                                eoc,
                                EOCBehaviors.Migrated,
                                false,
                                now);

                            newCallCodes.Add(callCode);
                            callCodesDictionary[callCode.Code] = callCode;
                        }
                        else
                        {
                            callCode = callCodesDictionary[eoc];
                        }

                        if (contact == null)
                        {

                        }

                        var callEndDate = call.CallDate
                            .Value.AddSeconds(call.CallDuration.Value);

                        var record = new Record
                        {
                            CreationDate = call.CallDate.Value,
                            LastUpdate = call.CallDate.Value,

                            StartTime = call.CallDate ?? now,
                            EndTime = callEndDate,
                            Duration = call.CallDuration.Value,

                            FormId = form.Id,
                            Form = form,

                            ContactId = contact.Id,
                            Contact = contact,

                            CallCodeId = callCode.Id,
                            CallCode = callCode,

                            UserId = agent?.Id,
                            User = agent,
                        };

                        newRecords.Add(record);

                        eUDFMore eMore = null;
                        if (eMoresDictionary.ContainsKey(contact.LeadId))
                            eMore = eMoresDictionary[contact.LeadId].LastOrDefault();

                        foreach (var question in orderedQuestions)
                        {
                            var answer = question.Order <= 16 ?
                                GetMasterAnswer(oldContact, question.Order) :
                                GetMoreAnswer(eMore, question.Order);

                            if (string.IsNullOrWhiteSpace(answer))
                                continue;

                            newAnsweredForms.Add(new AnsweredForm
                            {
                                Answer = answer,

                                CreationDate = now,
                                LastUpdate = now,

                                QuestionId = question.Id,
                                Question = question,
                              
                            });
                        }
                    }



                    if (!newCallCodes.Any() && !newContacts.Any() && !newRecords.Any() && !newAnsweredForms.Any())
                    {
                        Debug.WriteLine($"{campaign.Identifier} | skipped");
                        continue;
                    }

                    //newContext.Configuration.AutoDetectChangesEnabled = false;
                    newContext.CallCodes.AddRange(newCallCodes);
                    newContext.Contacts.AddRange(newContacts);
                    newContext.Records.AddRange(newRecords);
                    newContext.AnsweredForms.AddRange(newAnsweredForms);

                    await newContext.SaveChangesAsync();

                    Debug.WriteLine($"{campaign.Identifier}" +
                        $" | CallHistories: {callHistories.Count}" +
                        $" | Missing Leads: {callHistories.Count - newRecords.Count}" +
                        $" | Records: {newRecords.Count}" +
                        $" | Answers: {newAnsweredForms.Count}" +
                        $" | PlaceHolder: {notFoundAgents.Count}");

                    //newContext.Configuration.AutoDetectChangesEnabled = true;
                }
            }

            return true;
        }





        async Task<bool> TryCreatePlaceHolderAgent()
        {
            var placeHolder = "placeholder";

            var agent = await Context.Users
                .SingleOrDefaultAsync(x => x.Email == PlaceHolderAgentEmail);

            if (agent != null)
                return true;

            agent = new ApplicationUser
            {
                Email = PlaceHolderAgentEmail,
                UserName = PlaceHolderAgentEmail,

                Address = placeHolder,
                City = placeHolder,
                Contact = placeHolder,
                Country = placeHolder,
                Name = placeHolder,
                Notes = placeHolder,
                PhoneNumber = placeHolder,
                State = placeHolder,
                ZipCode = placeHolder,
                CreationDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                StartDate = DateTime.Now,
            };

            var owinContext = HttpContext.Current.GetOwinContext();
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();

            var result = await userManager.CreateAsync(agent, "Test123!");
            if (!result.Succeeded)
                return false;

            result = await userManager.AddToRoleAsync(agent.Id, "Agent");
            if (!result.Succeeded)
                return false;

            return true;
        }
        Entities.TimeZone GetTimeZoneByCode(string code, List<Entities.TimeZone> source)
        {
            code = code?.Trim();

            if (code == "EST")
                return source.SingleOrDefault(x => x.Code == "ET");

            if (code == "CST")
                return source.SingleOrDefault(x => x.Code == "CT");

            if (code == "PST")
                return source.SingleOrDefault(x => x.Code == "PT");

            if (code == "MST")
                return source.SingleOrDefault(x => x.Code == "MT");

            return source.SingleOrDefault(x => x.Code == "NONE");
        }
        string GetTempEmailFromAgentId(string name)
        {
            return name
                .Replace(" ", "_")
                .Replace("-", "_")
                .Replace(",", string.Empty) + "@intelemark.com";
        }
        Contact GetPlaceholderContact(int attempts,
            int leadId,
            Project project,
            Entities.TimeZone noneTimeZone,
            ApplicationUser agent)
        {
            return new Contact
            {
                Address = "Placeholder Address #123",
                AltAddress = "Alt Address #123",
                PhoneNumber = "6621123456",
                AltPhoneNumber = "6622123456",
                Company = "Placeholder Company",
                Name = "Placeholder Name",
                Notes = "Placeholder Notes",
                ZipCode = "99999",
                City = "Placeholder City",
                State = "Placeholder State",
                Country = "Placeholder Country",
                Title = "Placeholder Title",
                Extension = 9999,
                CreationDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Email = Guid.NewGuid().ToString("n") + "@intelemark.com",

                Attempts = attempts,
                LeadId = leadId,

                TimeZoneId = noneTimeZone.Id,
                TimeZone = noneTimeZone,
                TimeZoneString = noneTimeZone.Code,

                AgentId = agent.Id,
                Agent = agent,

                ProjectId = project.Id,
                Project = project,
            };
        }
    }
    public class NewGroup
    {
        public int CampaignId { set; get; }
        public string Identifier { set; get; }
        public int ProjectId { set; get; }
        public string Name { set; get; }
        public int Contacts { set; get; }
    }
    public class OldGroup
    {
        public string CampaignId { set; get; }
        public string ProjectId { set; get; }
        public int Contacts { set; get; }
    }
}
//async Task<bool> AgentsFix()
//{
//    var roles = await Context.Roles
//        .SingleOrDefaultAsync(x => x.Name == "Agent");

//    var users = await Context.Users
//        .Where(x => x.Roles.Any(r => r.RoleId == roles.Id))
//        .ToListAsync();

//    var oldAgents = await OldContext.etAgentMasters
//        .ToListAsync();

//    var newUsers = new List<ApplicationUser>();
//    foreach (var oldAgent in oldAgents)
//    {
//        var email = GetTempEmailFromAgentId(oldAgent.AgentID);

//        var user = users
//            .FirstOrDefault(x => x.Email == email);

//        if (user == null)
//            continue;

//        if (oldAgent.SecurityLevel != "Agent")
//            continue;

//        if (user.Address != oldAgent.Address)
//            user.Address = oldAgent.Address;

//        if (user.Name != oldAgent.AgentName)
//            user.Name = oldAgent.AgentName;

//        if (user.City != oldAgent.City)
//            user.City = oldAgent.City;

//        if (user.State != oldAgent.State)
//            user.State = oldAgent.State;

//        if (user.ZipCode != oldAgent.Zip)
//            user.ZipCode = oldAgent.Zip;

//        if (user.PhoneNumber != oldAgent.Phone)
//            user.PhoneNumber = oldAgent.Phone;

//        if (user.StartDate != oldAgent.AgentStartDate)
//            user.StartDate = oldAgent.AgentStartDate;

//        if (user.EndDate != oldAgent.AgentStopDate)
//            user.EndDate = oldAgent.AgentStopDate;

//        if (user.Notes != oldAgent.Notes)
//            user.Notes = oldAgent.Notes;

//        continue;
//    }

//    await Context.SaveChangesAsync();

//    return true;
//}
//async Task<bool> CheckContacts()
//{
//    Context.Database.CommandTimeout = 120;

//    var newItems = await Context.Projects
//        .Select(x => new
//        {
//            x.CampaignId,
//            ProjectId = x.Id,
//            Contacts = x.Contacts.Count()
//        })
//        .OrderByDescending(x => x.Contacts)
//        .ToListAsync();

//    var oldItems = await OldContext.elMasters
//        .GroupBy(x => new { x.CampaignId, x.ProjectId })
//        .Select(x => new
//        {
//            x.Key.CampaignId,
//            x.Key.ProjectId,
//            Contacts = x.Count()
//        })
//        .OrderByDescending(x => x.Contacts)
//        .ToListAsync();

//    var campaigns = await Context.Campaigns
//        .Include(x => x.Projects)
//        .ToListAsync();

//    foreach (var oldItem in oldItems)
//    {
//        var campaign = campaigns
//            .SingleOrDefault(x => x.Identifier.Equals(
//                oldItem.CampaignId, StringComparison.OrdinalIgnoreCase));

//        if (campaign == null)
//            continue;

//        var project = campaign.Projects
//            .SingleOrDefault(x => x.Name == oldItem.ProjectId);

//        if (project == null)
//            continue;

//        var newItem = newItems
//            .SingleOrDefault(x => x.CampaignId == campaign.Id
//                && x.ProjectId == project.Id);

//        if (newItem == null)
//            continue;

//        if (newItem.Contacts > oldItem.Contacts)
//        {

//        }

//        if (newItem.Contacts < oldItem.Contacts)
//        {

//        }


//        var diff = oldItem.Contacts - newItem.Contacts;
//        if (diff < 0)
//        {

//        }

//        if (diff > 0)
//            Missing += diff;
//    }

//    return true;
//}