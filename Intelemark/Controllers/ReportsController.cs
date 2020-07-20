//using Intelemark.DTO.DataInventory;
using Intelemark.Entities;
using Intelemark.Models;
using Intelemark.Models.Reports;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Intelemark.Controllers
{
    [Authorize]
    public class ReportsController : BaseController
    {
        private const string Empty = "N/A";
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<IEnumerable<Contact>> getContacts(IEnumerable<int> ids)
        {
            var contacts = await db.Contacts.Where(x => !x.IsDeleted && ids.Contains(x.Id)).ToListAsync();
            return contacts;
        }

        [Route("~/api/v2/admin/reports/data-inventory")]
        public async Task<JsonResult> GetDataInventory(DateTime? startDate, DateTime? endDate)
        {
            TelemetryConfiguration.Active.DisableTelemetry = true;

            //startDate = new DateTime(2010, 1, 1);
            startDate = DateTime.Now.AddDays(-7);
            endDate = DateTime.Now;

            try
            {


                //.OrderBy(x => x.StartTime)
                //.Skip(0)
                //.Take(100) ???? server side processing...

                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                try
                {

                    //READ: stackoverflow.com/questions/40155092/entity-framework-include-performance
                    var data = await DataInventoryLoad(startDate.Value, endDate.Value);

                    db.Database.CommandTimeout = 180;

                    var grouppings = await db.Contacts
                        .Where(x => !x.IsDeleted)
                        .GroupBy(x => new { x.ProjectId, x.AgentId })
                        .Select(x => new
                        {
                            x.Key.ProjectId,
                            x.Key.AgentId,
                            Detail = new DTO.DataInventory.DataInventoryDetail
                            {
                                Attempt0 = x.Count(c => c.Attempts == 0),
                                Attempt1 = x.Count(c => c.Attempts == 1),
                                Attempt2 = x.Count(c => c.Attempts == 2),
                                Attempt3 = x.Count(c => c.Attempts == 3),
                                Attempt4 = x.Count(c => c.Attempts == 4),
                                Attempt5 = x.Count(c => c.Attempts == 5),
                                Attempt6 = x.Count(c => c.Attempts == 6),
                                MoreThan6 = x.Count(c => c.Attempts > 6),
                                Final = x.Count(c => c.IsFinalized),
                                Hold = x.Count(c => c.InHold),
                                Total = x.Count(),
                            }
                        })
                        .OrderBy(x => x.ProjectId)
                        .OrderByDescending(x => x.Detail.Final)
                        .ToListAsync();

                    var projectsDictionary = new Dictionary<int, Dictionary<string, DTO.DataInventory.DataInventoryDetail>>();
                    foreach (var groupping in grouppings)
                    {
                        if (!projectsDictionary.ContainsKey(groupping.ProjectId))
                            projectsDictionary.Add(groupping.ProjectId, new Dictionary<string, DTO.DataInventory.DataInventoryDetail>());

                        var projectDictionary = projectsDictionary[groupping.ProjectId];

                        if (!projectDictionary.ContainsKey(groupping.AgentId ?? Empty))
                            projectDictionary.Add(groupping.AgentId ?? Empty, groupping.Detail);
                    }

                    //FEX: missing entity
                    //project-agent union

                    var output = new DTO.DataInventory.DataInventoryTotals();

                    foreach (var campaign in data.Campaigns)
                    {
                        var campaignOutput = new DTO.DataInventory.DataInventoryCampaign
                        {
                            Id = campaign.Id,
                            Name = campaign.Identifier,
                            Callbacks = data.Callbacks.Count(x => x.CampaignId == campaign.Id),
                        };

                        output.Data.Add(campaignOutput);

                        foreach (var project in campaign.Projects)
                        {
                            if (!projectsDictionary.ContainsKey(project.Id))
                                continue;

                            var projectDictionary = projectsDictionary[project.Id];

                            var projectOutput = new DTO.DataInventory.DataInventoryProject
                            {
                                Id = project.Id,
                                Name = project.Name,
                            };

                            campaignOutput.Data.Add(projectOutput);

                            projectOutput.Data.Add(new DTO.DataInventory.DataInventoryRow
                            {
                                Name = string.Empty,
                                Detail = projectDictionary.ContainsKey(Empty) ?
                                projectDictionary[Empty] : new DTO.DataInventory.DataInventoryDetail()
                            });

                            foreach (var agentId in projectDictionary.Keys)
                            {
                                if (agentId == Empty)
                                    continue;

                                var agent = db.Users.Find(new[] { agentId });

                                projectOutput.Data.Add(new DTO.DataInventory.DataInventoryRow
                                {
                                    Name = agent.Name,

                                    Detail = projectDictionary[agent.Id],

                                    Callbacks = data.Callbacks.Count(x => x.CampaignId == campaign.Id
                                        && x.AgentId == agent.Id),
                                });
                            }

                        }
                    }

                    output.Update();

                    System.Diagnostics.Debug.WriteLine("Start - {0}", GC.GetTotalMemory(false));
                    GC.Collect(0);
                    System.Diagnostics.Debug.WriteLine("End -  {0}", GC.GetTotalMemory(false));

                    return Json(output, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    AddAlert("Oops, Something Happened!", GetType().ToString(), "DataInventory", AlertType.error, ex);
                    return Json(new { Error = "Error!" });
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"fex42 - {ex.ToString()}");
                return Json(new { Error = "Error!" });
            }
        }

        [Route("~/api/v2/admin/reports/data-inventory-timezone")]
        public async Task<JsonResult> GetDataInventoryByTimeZone()
        {
            TelemetryConfiguration.Active.DisableTelemetry = true;

            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;

            try
            {
                var data = await DataInventoryByTimeZoneLoad();

                var groups = await db.Contacts
                    .Where(x => !x.IsDeleted)
                    .GroupBy(x => new { x.TimeZoneId, x.TimeZone.STD, x.ProjectId, x.Project.CampaignId })
                    .Select(x => new DTO.DataInventoryByTimeZone.Groupping
                    {
                        TimeZoneId = x.Key.TimeZoneId,
                        STD = x.Key.STD,
                        ProjectId = x.Key.ProjectId,
                        CampaignId = x.Key.CampaignId,
                        Detail = new DTO.DataInventoryByTimeZone.Detail
                        {
                            Attempt0 = x.Count(c => c.Attempts == 0),
                            Attempt1 = x.Count(c => c.Attempts == 1),
                            Attempt2 = x.Count(c => c.Attempts == 2),
                            Attempt3 = x.Count(c => c.Attempts == 3),
                            Attempt4 = x.Count(c => c.Attempts == 4),
                            Attempt5 = x.Count(c => c.Attempts == 5),
                            Attempt6 = x.Count(c => c.Attempts == 6),
                            MoreThan6 = x.Count(c => c.Attempts > 6),
                            Final = x.Count(c => c.IsFinalized),
                            Hold = x.Count(c => c.InHold),
                            Total = x.Count()
                        }
                    })
                    .OrderByDescending(x => x.STD)
                    .ToListAsync();

                var tdict = new Dictionary<int, DTO.DataInventoryByTimeZone.TimeZone>();
                var output = new DTO.DataInventoryByTimeZone.Output();
                foreach (var group in groups)
                {
                    if (!tdict.ContainsKey(group.TimeZoneId))
                    {
                        var timeZone = db.TimeZones.Find(group.TimeZoneId);
                        tdict.Add(timeZone.Id, new DTO.DataInventoryByTimeZone.TimeZone
                        {
                            Id = timeZone.Id,
                            Name = timeZone.Name,
                        });

                        output.Data.Add(tdict[timeZone.Id]);
                    }

                    var timeZoneOutput = tdict[group.TimeZoneId];

                    if (!timeZoneOutput.Dictionary.ContainsKey(group.ProjectId))
                    {
                        var project = db.Projects.Find(group.ProjectId);

                        var callbacks = data.Callbacks.ContainsKey(project.Id) ?
                            data.Callbacks[project.Id].ContainsKey(timeZoneOutput.Id) ?
                            data.Callbacks[project.Id][timeZoneOutput.Id] : 0 : 0;

                        timeZoneOutput.Dictionary.Add(project.Id, new DTO.DataInventoryByTimeZone.Project
                        {
                            Id = project.Id,
                            Name = project.Name,
                            Campaign = project.Campaign.Identifier, //see?
                            Detail = group.Detail,
                            Callbacks = callbacks,
                        });
                        timeZoneOutput.Data.Add(timeZoneOutput.Dictionary[project.Id]);
                    }
                }

                output.Update();

                System.Diagnostics.Debug.WriteLine("Start - {0}", GC.GetTotalMemory(false));
                GC.Collect(0);
                System.Diagnostics.Debug.WriteLine("End -  {0}", GC.GetTotalMemory(false));

                return Json(output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"fex42 - {ex.ToString()}");
                return Json(new { Error = "Error!" });
            }
        }

        public ActionResult DataInventory()
        {
            return View();
        }

        public ActionResult DataInventoryByTimeZone()
        {
            return View();
        }

        public async Task<ActionResult> AgentProduction()
        {
            try
            {

                var StartDate = DateTime.Now.AddDays(-1);
                var StartDateT = StartDate.AddDays(-3);
                var StartDateTen = StartDate.AddDays(-10);
                var EndDate = DateTime.Now;
                var AgentLogs = await db.AgentLogs.Where(x => x.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var AgentsToReport = AgentLogs.GroupBy(x => new { x.Agent, x.Campaign }).Select(x => x.Key).ToList();
                var Records = await db.Records.Where(x => x.Form.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var ThreeDay = await db.Records.Where(x => x.Form.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDateT) && !x.IsDeleted).ToListAsync();
                var TenDay = await db.Records.Where(x => x.Form.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDateTen) && !x.IsDeleted).ToListAsync();
                var UserCampaigns = await db.UserCampaigns.Where(x => x.CampaignId == -1 && !x.IsDeleted).ToListAsync();
                TimeSpan TotalTicks = DateTime.Now - DateTime.Now.AddDays(-7);

                var AgentProductionModel1 = AgentsToReport.Select(x => new AgentProductionViewModel
                {
                    Agent = x.Agent,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    UserCampaign = UserCampaigns.FirstOrDefault(y => y.UserId == x.Agent.Id && y.Campaign.Id == x.Campaign.Id) ?? new UserCampaign(),
                    ReportedHours = AgentLogs.Where(y => y.AgentId == x.Agent.Id && y.Campaign.Id == x.Campaign.Id).Sum(z => z.DialingHours),
                    ReportedTrainingHours = AgentLogs.Where(y => y.AgentId == x.Agent.Id && y.Campaign.Id == x.Campaign.Id).Sum(z => z.TrainingHours),
                    DialsPerHour = Records.Where(y => y.UserId == x.Agent.Id && y.Form.Campaign.Id == x.Campaign.Id).AsEnumerable().Count() / TotalTicks.TotalHours,
                    SuccessPerHour = Records.Where(y => y.UserId == x.Agent.Id && y.CallCode.IsSuccess && y.Form.Campaign.Id == x.Campaign.Id).AsEnumerable().Count() / TotalTicks.TotalHours,
                    ThreeDaySuccessPerHour = Records.Where(y => y.UserId == x.Agent.Id && y.CallCode.IsSuccess && y.Form.Campaign.Id == x.Campaign.Id).AsEnumerable().Count() / TotalTicks.Add(new TimeSpan(3, 0, 0, 0)).TotalHours,
                    TenDaySuccessPerHour = Records.Where(y => y.UserId == x.Agent.Id && y.CallCode.IsSuccess && y.Form.Campaign.Id == x.Campaign.Id).AsEnumerable().Count() / TotalTicks.Add(new TimeSpan(10, 0, 0, 0)).TotalHours,
                    Campaign = x.Campaign
                });

                var newModel = AgentProductionModel1.GroupBy(x => x.Campaign).Select(g => new AgentProductionViewModelByCampaign
                {
                    Campaign = g.Key,
                    CampaignProduction = g.ToList(),
                    StartDate = StartDate,
                    EndDate = EndDate,
                });

                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier");
                return View(newModel);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "AgentProduction", AlertType.error, ex);
                return View(new AgentProductionViewModel());
            }

        }

        [HttpPost]
        public async Task<ActionResult> AgentProduction(BaseReportModel model)
        {
            try
            {
                var StartDate = model.StartDate;
                var EndDate = model.EndDate.AddHours(23.99);
                var StartDateT = StartDate.AddDays(-3);
                var StartDateTen = StartDate.AddDays(-10);


                var AgentLogs = await db.AgentLogs.Where(x => (x.CampaignId == model.CampaignId || model.CampaignId == -1) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var AgentsToReport = AgentLogs.GroupBy(x => new { x.Agent, x.Campaign }).Select(x => x.Key).ToList();
                var Records = await db.Records.Where(x => (x.Form.CampaignId == model.CampaignId || model.CampaignId == -1) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var ThreeDay = await db.Records.Where(x => x.Form.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDateT) && !x.IsDeleted).ToListAsync();
                var TenDay = await db.Records.Where(x => x.Form.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDateTen) && !x.IsDeleted).ToListAsync();
                var UserCampaigns = await db.UserCampaigns.Where(x => (x.CampaignId == model.CampaignId || model.CampaignId == -1) && !x.IsDeleted).ToListAsync();
                TimeSpan TotalTicks = EndDate - StartDate;

                var AgentProductionModel = AgentsToReport.Select(x => new AgentProductionViewModel
                {
                    Agent = x.Agent,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    UserCampaign = UserCampaigns.FirstOrDefault(y => y.UserId == x.Agent.Id && y.Campaign.Id == x.Campaign.Id) ?? new UserCampaign(),
                    ReportedHours = AgentLogs.Where(y => y.AgentId == x.Agent.Id && y.Campaign.Id == x.Campaign.Id).Sum(z => z.DialingHours),
                    ReportedTrainingHours = AgentLogs.Where(y => y.AgentId == x.Agent.Id && y.Campaign.Id == x.Campaign.Id).Sum(z => z.TrainingHours),
                    DialsPerHour = Records.Where(y => y.UserId == x.Agent.Id && y.Form.Campaign.Id == x.Campaign.Id).AsEnumerable().Count() / TotalTicks.TotalHours,
                    SuccessPerHour = Records.Where(y => y.UserId == x.Agent.Id && y.CallCode.IsSuccess && y.Form.Campaign.Id == x.Campaign.Id).AsEnumerable().Count() / TotalTicks.TotalHours,
                    ThreeDaySuccessPerHour = Records.Where(y => y.UserId == x.Agent.Id && y.CallCode.IsSuccess && y.Form.Campaign.Id == x.Campaign.Id).AsEnumerable().Count() / TotalTicks.Add(new TimeSpan(3, 0, 0, 0)).TotalHours,
                    TenDaySuccessPerHour = Records.Where(y => y.UserId == x.Agent.Id && y.CallCode.IsSuccess && y.Form.Campaign.Id == x.Campaign.Id).AsEnumerable().Count() / TotalTicks.Add(new TimeSpan(10, 0, 0, 0)).TotalHours,
                    Campaign = x.Campaign
                });

                var newModel = AgentProductionModel.GroupBy(x => x.Campaign).Select(g => new AgentProductionViewModelByCampaign
                {
                    Campaign = g.Key,
                    CampaignProduction = g.ToList(),
                    StartDate = StartDate,
                    EndDate = EndDate,
                }).ToList();
                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier", model.CampaignId);
                return View(newModel);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "AgentProduction", AlertType.error, ex);
                return View(new AgentProductionViewModel());
            }
        }

        public async Task<ActionResult> CampaignRevenueReport()
        {
            try
            {
                var StartDate = DateTime.Now.AddDays(-1);
                var EndDate = DateTime.Now;
                var userID = User.Identity.GetUserId();
                TimeSpan TotalTicks = StartDate - EndDate;

                var Campaigns = db.UserCampaigns.Where(x => x.UserId == userID).Select(c => c.Campaign);
                var AgentLogs = await db.AgentLogs.Where(x => Campaigns.Contains(x.Campaign) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var AgentsToReport = AgentLogs.GroupBy(x => x.Agent).Select(x => x.Key).ToList();
                var Records = await db.Records.Where(x => Campaigns.Contains(x.Form.Campaign) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var UserCampaigns = await db.UserCampaigns.Where(x => Campaigns.Contains(x.Campaign) && !x.IsDeleted).ToListAsync();

                var AgentProductionModel = Campaigns.Select(x => new CampaignRevenueViewModel
                {
                    StartDate = StartDate,
                    EndDate = StartDate,
                    Campaign = x,
                    CampaignHours = AgentLogs.Where(y => y.CampaignId == x.Id).Select(z => z.DialingHours).DefaultIfEmpty(0).Sum(),
                    BudgetedHours = UserCampaigns.Where(y => y.CampaignId == x.Id).Select(z => z.BudgetedHours).DefaultIfEmpty(0).Sum(),
                    TrainingHours = AgentLogs.Where(y => y.CampaignId == x.Id).Select(z => z.TrainingHours).DefaultIfEmpty(0).Sum(),
                    SuccessesPerHour = Records.Where(y => y.Form.CampaignId == x.Id && y.CallCode.IsSuccess).AsEnumerable().Count() / TotalTicks.TotalHours,
                    Successes = Records.Where(y => y.Form.CampaignId == x.Id && y.CallCode.IsSuccess).AsEnumerable().Count(),
                    AgentProductionList = AgentLogs.Where(y => y.CampaignId == x.Id).Select(y => new AgentProductionViewModel
                    {
                        UserCampaign = UserCampaigns.FirstOrDefault(z => z.UserId == y.AgentId && z.CampaignId == x.Id) ?? null,
                        ReportedHours = AgentLogs.Where(z => z.AgentId == y.AgentId).Sum(z => z.DialingHours),
                        ReportedTrainingHours = AgentLogs.Where(z => z.AgentId == y.AgentId).Sum(z => z.TrainingHours),
                        DialsPerHour = Records.Where(z => z.UserId == y.AgentId).AsEnumerable().Count() / TotalTicks.TotalHours,
                        SuccessPerHour = Records.Where(z => z.UserId == y.AgentId && z.CallCode.IsSuccess).AsEnumerable().Count() / TotalTicks.TotalHours
                    }).ToList()
                });

                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier");
                return View(AgentProductionModel);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "CampaignRevenueReport", AlertType.error, ex);
                return View(new AgentProductionViewModel());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CampaignRevenueReport(BaseReportModel model)
        {
            try
            {
                var StartDate = model.StartDate;
                var EndDate = model.EndDate.AddHours(23.99);
                var userID = User.Identity.GetUserId();
                var isAdmin = User.IsInRole("Admin");
                TimeSpan TotalTicks = EndDate - StartDate;
                var Campaigns = await db.UserCampaigns.Where(x => (x.UserId == userID || isAdmin) && (x.CampaignId == model.CampaignId || model.CampaignId == -1)).Select(c => c.Campaign).Distinct().ToListAsync();
                var CampaignIds = Campaigns.Select(c => c.Id);
                var AgentLogs = await db.AgentLogs.Where(x => CampaignIds.Contains(x.Campaign.Id) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var AgentsToReport = AgentLogs.GroupBy(x => x.Agent).Select(x => x.Key).ToList();
                var Records = await db.Records.Where(x => CampaignIds.Contains(x.Form.CampaignId) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var UserCampaigns = await db.UserCampaigns.Where(x => CampaignIds.Contains(x.Campaign.Id) && !x.IsDeleted).ToListAsync();

                var AgentProductionModel = Campaigns.Select(x => new CampaignRevenueViewModel
                {
                    StartDate = StartDate,
                    EndDate = StartDate,
                    Campaign = x,
                    CampaignHours = AgentLogs.Where(y => y.CampaignId == x.Id).Select(z => z.DialingHours).DefaultIfEmpty(0).Sum(),
                    BudgetedHours = UserCampaigns.Where(y => y.CampaignId == x.Id).Select(z => z.BudgetedHours).DefaultIfEmpty(0).Sum(),
                    TrainingHours = AgentLogs.Where(y => y.CampaignId == x.Id).Select(z => z.TrainingHours).DefaultIfEmpty(0).Sum(),
                    SuccessesPerHour = Records.Where(y => y.Form.CampaignId == x.Id && y.CallCode.IsSuccess).AsEnumerable().Count() / TotalTicks.TotalHours,
                    Successes = Records.Where(y => y.Form.CampaignId == x.Id && y.CallCode.IsSuccess).AsEnumerable().Count(),
                    AgentProductionList = AgentLogs.Where(y => y.CampaignId == x.Id).Select(y => new AgentProductionViewModel
                    {
                        UserCampaign = UserCampaigns.FirstOrDefault(z => z.UserId == y.AgentId) ?? new UserCampaign(),
                        ReportedHours = AgentLogs.Where(z => z.AgentId == y.AgentId).Sum(z => z.DialingHours),
                        ReportedTrainingHours = AgentLogs.Where(z => z.AgentId == y.AgentId).Sum(z => z.TrainingHours),
                        DialsPerHour = Records.Where(z => z.UserId == y.AgentId).AsEnumerable().Count() / TotalTicks.TotalHours,
                        SuccessPerHour = Records.Where(z => z.UserId == y.AgentId && z.CallCode.IsSuccess).AsEnumerable().Count() / TotalTicks.TotalHours
                    }).ToList()
                });

                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(Campaigns, "Id", "Identifier", model.CampaignId);
                return View(AgentProductionModel);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "CampaignRevenueReport", AlertType.error, ex);
                return View(new AgentProductionViewModel());
            }
        }

        public ActionResult CampaignRevenueReportByDay()
        {
            try
            {
                var Campaigns = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                Campaigns.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(Campaigns, "Id", "Identifier");
                return View(new List<CampaignRevenueViewModelByDay>());
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "CampaignRevenueReportByDay", AlertType.error, ex);
                return View(new List<CampaignRevenueViewModelByDay>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CampaignRevenueReportByDay(BaseReportModel model)
        {
            try
            {
                var StartDate = model.StartDate;
                var EndDate = model.EndDate.AddHours(23.99);
                var userID = User.Identity.GetUserId();
                var isAdmin = User.IsInRole("Admin");
                TimeSpan TotalTicks = EndDate - StartDate;
                var Campaigns = await db.UserCampaigns.Where(x => (x.UserId == userID || isAdmin) && (x.CampaignId == model.CampaignId || model.CampaignId == -1)).Select(c => c.Campaign).Distinct().ToListAsync();
                var CampaignIds = Campaigns.Select(c => c.Id);
                var AgentLogs = await db.AgentLogs.Where(x => CampaignIds.Contains(x.Campaign.Id) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var AgentsToReport = AgentLogs.GroupBy(x => x.Agent).Select(x => x.Key).ToList();
                var Records = await db.Records.Where(x => CampaignIds.Contains(x.Form.CampaignId) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var UserCampaigns = await db.UserCampaigns.Where(x => CampaignIds.Contains(x.Campaign.Id) && !x.IsDeleted).ToListAsync();


                var modellist = new List<CampaignRevenueViewModelByDay>();

                for (int i = 0; i < Math.Ceiling(TotalTicks.TotalDays); i++)
                {
                    var StartDateDay = model.StartDate.Date.AddDays(i);
                    var EndDateDay = model.StartDate.AddDays(i).AddHours(23.99);
                    var AgentLogsByDay = await db.AgentLogs.Where(x => (x.CreationDate <= EndDateDay && x.CreationDate >= StartDateDay)).ToListAsync();
                    var RecordsByDay = await db.Records.Where(x => CampaignIds.Contains(x.Form.CampaignId) && (x.CreationDate <= EndDateDay && x.CreationDate >= StartDateDay) && !x.IsDeleted).ToListAsync();

                    var Revenue = Campaigns.Select(x => new CampaignRevenueViewModel
                    {
                        Campaign = x,
                        CampaignHours = AgentLogsByDay.Where(y => y.CampaignId == x.Id).Select(z => z.DialingHours).DefaultIfEmpty(0).Sum(),
                        BudgetedHours = UserCampaigns.Where(y => y.CampaignId == x.Id).Select(z => z.BudgetedHours).DefaultIfEmpty(0).Sum(),
                        TrainingHours = AgentLogsByDay.Where(y => y.CampaignId == x.Id).Select(z => z.TrainingHours).DefaultIfEmpty(0).Sum(),
                        SuccessesPerHour = RecordsByDay.Where(y => y.Form.CampaignId == x.Id && y.CallCode.IsSuccess).AsEnumerable().Count() / TotalTicks.TotalHours,
                        Successes = RecordsByDay.Where(y => y.Form.CampaignId == x.Id && y.CallCode.IsSuccess).AsEnumerable().Count(),
                        AgentProductionList = AgentLogs.Where(y => y.CampaignId == x.Id).Select(y => new AgentProductionViewModel
                        {
                            UserCampaign = UserCampaigns.FirstOrDefault(z => z.UserId == y.AgentId) ?? new UserCampaign(),
                            ReportedHours = AgentLogsByDay.Where(z => z.AgentId == y.AgentId).Select(z => z.DialingHours).DefaultIfEmpty(0).Sum(),
                            ReportedTrainingHours = AgentLogsByDay.Where(z => z.AgentId == y.AgentId).Select(z => z.TrainingHours).DefaultIfEmpty(0).Sum(),
                            DialsPerHour = RecordsByDay.Where(z => z.UserId == y.AgentId && (x.CreationDate <= EndDateDay && x.CreationDate >= StartDateDay)).AsEnumerable().Count() / TotalTicks.TotalHours,
                            SuccessPerHour = RecordsByDay.Where(z => z.UserId == y.AgentId && z.CallCode.IsSuccess).AsEnumerable().Count() / 24
                        }).ToList()
                    });
                    modellist.Add(new CampaignRevenueViewModelByDay { StartDate = StartDate, EndDate = EndDate, ReportDay = StartDateDay, CampaignRevenueReport = Revenue });
                }

                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(Campaigns, "Id", "Identifier", model.CampaignId);
                return View(modellist);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "CampaignRevenueReportByDay", AlertType.error, ex);
                return View(new List<CampaignRevenueViewModelByDay>());
            }
        }

        public async Task<ActionResult> AgentCallAnalysis()
        {
            try
            {
                var StartDate = DateTime.Now.AddDays(-1);
                var EndDate = DateTime.Now;
                var AgentLogs = await db.AgentLogs.Where(x => x.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var AgentsToReport = AgentLogs.GroupBy(x => x.Agent).Select(x => x.Key).ToList();
                var Records = await db.Records.Where(x => x.Form.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var UserCampaigns = await db.UserCampaigns.Where(x => x.CampaignId == -1 && !x.IsDeleted).ToListAsync();
                var UserHistory = await db.UsersHistory.Where(x => x.CreationDate <= EndDate && x.CreationDate >= StartDate && !x.IsDeleted).ToListAsync();

                TimeSpan TotalTicks = DateTime.Now - DateTime.Now.AddDays(-7);

                var AgentProductionModel1 = AgentsToReport.Select(x => new AgentCallAnalysisViewModel
                {
                    Agent = x,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    UserCampaign = UserCampaigns.FirstOrDefault(y => y.UserId == x.Id) ?? new UserCampaign(),
                    ReportedHours = AgentLogs.Where(y => y.AgentId == x.Id).Sum(z => z.DialingHours),
                    ReportedTrainingHours = AgentLogs.Where(y => y.AgentId == x.Id).Sum(z => z.TrainingHours),
                    DialsPerHour = Records.Where(y => y.UserId == x.Id).AsEnumerable().Count() / TotalTicks.TotalHours,
                    SuccessPerHour = Records.Where(y => y.UserId == x.Id && y.CallCode.IsSuccess).AsEnumerable().Count() / TotalTicks.TotalHours,
                    ActualHours = UserHistory.Where(y => y.User.Id == x.Id).Select(m => m.Duration).DefaultIfEmpty(0).Sum() / 3600,
                    MinimumCallTime = Records.Where(y => y.UserId == x.Id).Select(m => m.Duration).DefaultIfEmpty(0).Min(),
                    MaximumCallTime = Records.Where(y => y.UserId == x.Id).Select(m => m.Duration).DefaultIfEmpty(0).Max()
                });

                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier");
                return View(AgentProductionModel1);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "AgentProduction", AlertType.error, ex);
                return View(new AgentCallAnalysisViewModel());
            }

        }

        [HttpPost]
        public async Task<ActionResult> AgentCallAnalysis(BaseReportModel model)
        {
            try
            {
                var StartDate = model.StartDate;
                var EndDate = model.EndDate.AddHours(23.99);


                var AgentLogs = await db.AgentLogs.Where(x => (x.CampaignId == model.CampaignId || model.CampaignId == -1) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var AgentsToReport = AgentLogs.GroupBy(x => x.Agent).Select(x => x.Key).ToList();
                var Records = await db.Records.Where(x => (x.Form.CampaignId == model.CampaignId || model.CampaignId == -1) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var UserCampaigns = await db.UserCampaigns.Where(x => (x.CampaignId == model.CampaignId || model.CampaignId == -1) && !x.IsDeleted).ToListAsync();
                var UserHistory = await db.UsersHistory.Where(x => x.CreationDate <= EndDate && x.CreationDate >= StartDate && !x.IsDeleted).ToListAsync();
                TimeSpan TotalTicks = EndDate - StartDate;

                var output = AgentsToReport.Select(x => new AgentCallAnalysisViewModel
                {
                    Agent = x,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    UserCampaign = UserCampaigns.FirstOrDefault(y => y.UserId == x.Id) ?? new UserCampaign(),
                    ReportedHours = AgentLogs.Where(y => y.AgentId == x.Id).Sum(z => z.DialingHours),
                    ReportedTrainingHours = AgentLogs.Where(y => y.AgentId == x.Id).Sum(z => z.TrainingHours),
                    DialsPerHour = Records.Where(y => y.UserId == x.Id).AsEnumerable().Count() / TotalTicks.TotalHours,
                    TotalDials = Records.Where(y => y.UserId == x.Id).AsEnumerable().Count(),
                    SuccessPerHour = Records.Where(y => y.UserId == x.Id && y.CallCode.IsSuccess).AsEnumerable().Count() / TotalTicks.TotalHours,
                    ActualHours = UserHistory.Where(y => y.User.Id == x.Id).Select(m => m.Duration).DefaultIfEmpty(0).Sum() / 3600,
                    MinimumCallTime = Records.Where(y => y.UserId == x.Id).Select(m => m.Duration).DefaultIfEmpty(0).Min(),
                    MaximumCallTime = Records.Where(y => y.UserId == x.Id).Select(m => m.Duration).DefaultIfEmpty(0).Max()
                });

                var campaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                campaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(campaignsSelectList, "Id", "Identifier", model.CampaignId);

                return View(output);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "AgentProduction", AlertType.error, ex);
                return View(new AgentCallAnalysisViewModel());
            }
        }

        public async Task<ActionResult> PayRollPrep()
        {
            try
            {
                var StartDate = DateTime.Now.AddDays(-1);
                var EndDate = DateTime.Now;
                var AgentLogs = await db.AgentLogs.Where(x => x.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var AgentsToReport = AgentLogs.GroupBy(x => x.Agent).Select(x => x.Key).ToList();
                var Records = await db.Records.Where(x => x.Form.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var UserCampaigns = await db.UserCampaigns.Where(x => x.CampaignId == -1 && !x.IsDeleted).ToListAsync();
                TimeSpan TotalTicks = DateTime.Now - DateTime.Now.AddDays(-7);

                var AgentProductionModel1 = AgentsToReport.Select(x => new AgentProductionViewModel
                {
                    Agent = x,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    UserCampaign = UserCampaigns.FirstOrDefault(y => y.UserId == x.Id) ?? new UserCampaign(),
                    ReportedHours = AgentLogs.Where(y => y.AgentId == x.Id).Sum(z => z.DialingHours),
                    ReportedTrainingHours = AgentLogs.Where(y => y.AgentId == x.Id).Sum(z => z.TrainingHours),
                    DialsPerHour = Records.Where(y => y.UserId == x.Id).AsEnumerable().Count() / TotalTicks.TotalHours,
                    SuccessPerHour = Records.Where(y => y.UserId == x.Id && y.CallCode.IsSuccess).AsEnumerable().Count() / TotalTicks.TotalHours
                });

                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier");
                return View(new List<PayRollPrepByDay>());
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "AgentProduction", AlertType.error, ex);
                return View(new List<PayRollPrepByDay>());
            }

        }

        [HttpPost]
        public async Task<ActionResult> PayRollPrep(BaseReportModel model)
        {
            try
            {
                var StartDate = model.StartDate;
                var EndDate = model.EndDate.AddHours(23.99);
                var AgentLogs = await db.AgentLogs.Where(x => (x.CampaignId == model.CampaignId || model.CampaignId == -1) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var AgentsToReport = AgentLogs.GroupBy(z => z.Campaign).Select(x => x.First().Agent).ToList();
                var Records = await db.Records.Where(x => (x.Form.CampaignId == model.CampaignId || model.CampaignId == -1) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var UserCampaigns = await db.UserCampaigns.Where(x => (x.CampaignId == model.CampaignId || model.CampaignId == -1) && !x.IsDeleted).ToListAsync();
                TimeSpan TotalTicks = EndDate - StartDate;

                var modellist = new List<PayRollPrepByDay>();
                for (int i = 0; i < Math.Ceiling(TotalTicks.TotalDays); i++)
                {
                    var StartDateDay = model.StartDate.Date.AddDays(i);
                    var EndDateDay = model.StartDate.AddDays(i).AddHours(23.99);
                    var AgentLogsByDay = db.AgentLogs.Where(x => (x.CreationDate <= EndDateDay && x.CreationDate >= StartDateDay));
                    var AgentProductionModel = UserCampaigns.Select(x => new PayRollPrep
                    {
                        Agent = x.User,
                        StartDate = StartDate,
                        EndDate = EndDate,
                        UserCampaign = x,
                        ReportedHours = AgentLogsByDay.Where(y => y.AgentId == x.UserId).Select(z => z.DialingHours).DefaultIfEmpty(0).Sum(),
                        ReportedTrainingHours = AgentLogsByDay.Where(y => y.AgentId == x.UserId).Select(z => z.DialingHours).DefaultIfEmpty(0).Sum()
                    });
                    modellist.Add(new PayRollPrepByDay { StartDate = StartDate, EndDate = EndDate, ReportDay = StartDateDay, AgentReport = AgentProductionModel });
                }
                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier", model.CampaignId);
                return View(modellist);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "AgentProduction", AlertType.error, ex);
                return View(new AgentProductionViewModel());
            }
        }

        public async Task<ActionResult> AgentAvailability()
        {
            try
            {
                var UserCampaigns = await db.UserCampaigns.Where(x => x.CampaignId == -1 && !x.IsDeleted).ToListAsync();
                var AvailableHours = await db.Users.Where(x => !x.IsDeleted && x.Roles.Any(y => y.RoleId == "2" || y.RoleId == "3")).ToListAsync();
                var AgentAvailabilityModel = AvailableHours.Select(x => new AgentAvailabilityViewModel
                {
                    Agent = x,
                    AvailableHours = x.AvailableHours,
                    BudgetedHours = UserCampaigns.Where(z => z.UserId == x.Id).Select(o => o.BudgetedHours).DefaultIfEmpty(0).Sum(),
                    Status = x.AvailableHours > UserCampaigns.Where(z => z.UserId == x.Id).Select(o => o.BudgetedHours).DefaultIfEmpty(0).Sum() ? "LOW" : (x.AvailableHours < UserCampaigns.Where(z => z.UserId == x.Id).Select(o => o.BudgetedHours).DefaultIfEmpty(0).Sum() ? "HIGH" : "ON POINT")
                });

                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier");
                return View(AgentAvailabilityModel);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "AgentAvailability", AlertType.error, ex);
                return View(new AgentAvailabilityViewModel());
            }

        }

        [HttpPost]
        public async Task<ActionResult> AgentAvailability(BaseReportModel model)
        {
            try
            {
                var UserCampaigns = await db.UserCampaigns.Where(x => (x.CampaignId == model.CampaignId || model.CampaignId == -1) && !x.IsDeleted).ToListAsync();
                var AvailableHours = await db.Users.Where(x => !x.IsDeleted && x.Roles.Any(y => y.RoleId == "2" || y.RoleId == "3")).ToListAsync();
                var AgentAvailabilityModel = AvailableHours.Select(x => new AgentAvailabilityViewModel
                {
                    Agent = x,
                    AvailableHours = x.AvailableHours,
                    BudgetedHours = UserCampaigns.Where(z => z.UserId == x.Id).Select(o => o.BudgetedHours).DefaultIfEmpty(0).Sum(),
                    Status = x.AvailableHours > UserCampaigns.Where(z => z.UserId == x.Id).Select(o => o.BudgetedHours).DefaultIfEmpty(0).Sum() ? "Low Assignments" : (x.AvailableHours < UserCampaigns.Where(z => z.UserId == x.Id).Select(o => o.BudgetedHours).DefaultIfEmpty(0).Sum() ? "High Assignments" : "On Point")

                });

                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier");
                return View(AgentAvailabilityModel);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "AgentAvailability", AlertType.error, ex);
                return View(new CallAnalysisByTimeOfDayViewModel());
            }

        }

        public async Task<ActionResult> CallAnalysisByTimeOfDay()
        {
            try
            {
                var StartDate = DateTime.Now.AddDays(-1);
                var EndDate = DateTime.Now;

                var Records = await db.Records.Where(x => x.Form.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                TimeSpan TotalTicks = DateTime.Now - DateTime.Now.AddDays(-7);
                var modelTimeOfDay = new List<CallAnalysisByTimeOfDayGroup>();

                for (int i = 0; i < TotalTicks.TotalHours; i++)
                {
                    var newStartDate = StartDate.AddHours(i);
                    var newEndDate = StartDate.AddHours(i).AddMinutes(59.99);
                    var CallAnalysisTimeDay = Records.Where(z => (z.CreationDate >= newStartDate && z.CreationDate <= newEndDate)).GroupBy(o => o.User).Select(x => new CallAnalysisByTimeOfDayViewModel
                    {
                        Agent = x.Key,
                        StartDate = StartDate,
                        EndDate = EndDate,
                        TotalDials = x.Count(),
                        Successes = x.Sum(l => l.CallCode.IsSuccess ? 1 : 0),
                        Completes = x.Sum(l => l.CallCode.Behavior == Utilities.EOCBehaviors.FinalizeRecord || l.CallCode.Behavior == Utilities.EOCBehaviors.FinalizeCompany ? 1 : 0)
                    }).ToList();

                    modelTimeOfDay.Add(new CallAnalysisByTimeOfDayGroup { CallAnalyisisGroupedList = CallAnalysisTimeDay, Hours = newStartDate, StartDate = StartDate, EndDate = EndDate });
                }


                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier");
                return View(modelTimeOfDay);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "CallAnalysisByTimeOfDay", AlertType.error, ex);
                return View(new CallAnalysisByTimeOfDayViewModel());
            }

        }

        [HttpPost]
        public async Task<ActionResult> CallAnalysisByTimeOfDay(BaseReportModel model)
        {
            try
            {
                var StartDate = model.StartDate;
                var EndDate = model.EndDate.AddHours(23.99);
                var Records = await db.Records.Where(x => (x.Form.CampaignId == model.CampaignId || model.CampaignId == -1) && (x.CreationDate <= model.EndDate && x.CreationDate >= model.StartDate) && !x.IsDeleted).ToListAsync();

                TimeSpan TotalTicks = EndDate - StartDate;
                var modelTimeOfDay = new List<CallAnalysisByTimeOfDayGroup>();

                for (int i = 0; i < TotalTicks.TotalHours; i++)
                {
                    var newStartDate = StartDate.AddHours(i);
                    var newEndDate = StartDate.AddHours(i).AddMinutes(59.99);
                    var CallAnalysisTimeDay = Records.Where(z => (z.CreationDate >= newStartDate && z.CreationDate <= newEndDate)).GroupBy(o => o.User).Select(x => new CallAnalysisByTimeOfDayViewModel
                    {
                        Agent = x.Key,
                        StartDate = StartDate,
                        EndDate = EndDate,
                        TotalDials = x.Count(),
                        Successes = x.Sum(d => d.CallCode.IsSuccess ? 1 : 0),
                        Completes = x.Sum(l => l.CallCode.Behavior == Utilities.EOCBehaviors.FinalizeRecord || l.CallCode.Behavior == Utilities.EOCBehaviors.FinalizeCompany ? 1 : 0)
                    }).ToList();

                    modelTimeOfDay.Add(new CallAnalysisByTimeOfDayGroup { CallAnalyisisGroupedList = CallAnalysisTimeDay, Hours = newStartDate, StartDate = model.StartDate, EndDate = model.EndDate });
                }


                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier", model.CampaignId);
                return View(modelTimeOfDay);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "CallAnalysisByTimeOfDay", AlertType.error, ex);
                return View(new CallAnalysisByTimeOfDayViewModel());
            }
        }

        public async Task<ActionResult> PayRoll()
        {
            try
            {
                var StartDate = DateTime.Now.AddDays(-1);
                var EndDate = DateTime.Now;
                var AgentLogs = await db.AgentLogs.Where(x => x.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var AgentsToReport = AgentLogs.GroupBy(x => x.Agent).Select(x => x.Key).ToList();
                var Records = await db.Records.Where(x => x.Form.CampaignId == -1 && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var UserCampaigns = await db.UserCampaigns.Where(x => x.CampaignId == -1 && !x.IsDeleted).ToListAsync();
                TimeSpan TotalTicks = DateTime.Now - DateTime.Now.AddDays(-7);

                var AgentProductionModel1 = AgentsToReport.Select(x => new AgentProductionViewModel
                {
                    Agent = x,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    UserCampaign = UserCampaigns.FirstOrDefault(y => y.UserId == x.Id) ?? new UserCampaign(),
                    ReportedHours = AgentLogs.Where(y => y.AgentId == x.Id).Sum(z => z.DialingHours),
                    ReportedTrainingHours = AgentLogs.Where(y => y.AgentId == x.Id).Sum(z => z.TrainingHours),
                    DialsPerHour = Records.Where(y => y.UserId == x.Id).AsEnumerable().Count() / TotalTicks.TotalHours,
                    SuccessPerHour = Records.Where(y => y.UserId == x.Id && y.CallCode.IsSuccess).AsEnumerable().Count() / TotalTicks.TotalHours
                });

                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier");
                return View(new List<PayRoll>());
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "AgentProduction", AlertType.error, ex);
                return View(new List<PayRoll>());
            }

        }

        [HttpPost]
        public async Task<ActionResult> PayRoll(BaseReportModel model)
        {
            try
            {
                var StartDate = model.StartDate;
                var EndDate = model.EndDate.AddHours(23.99);
                var AgentLogs = await db.AgentLogs.Where(x => (x.CampaignId == model.CampaignId || model.CampaignId == -1) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var AgentsToReport = AgentLogs.GroupBy(x => x.Agent).Select(x => x.Key).ToList();
                var Records = await db.Records.Where(x => (x.Form.CampaignId == model.CampaignId || model.CampaignId == -1) && (x.CreationDate <= EndDate && x.CreationDate >= StartDate) && !x.IsDeleted).ToListAsync();
                var UserCampaigns = await db.UserCampaigns.Where(x => (x.CampaignId == model.CampaignId || model.CampaignId == -1) && !x.IsDeleted).ToListAsync();
                TimeSpan TotalTicks = EndDate - StartDate;
                var Penalties = await db.Penalties.Where(x => !x.IsDeleted).ToListAsync();
                var AgentProductionModel = UserCampaigns.Select(x => new PayRoll
                {
                    Agent = x.User,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    UserCampaign = x,
                    ReportedHours = AgentLogs.Where(y => y.AgentId == x.UserId).Select(z => z.DialingHours).DefaultIfEmpty(0).Sum(),
                    ReportedTrainingHours = AgentLogs.Where(y => y.AgentId == x.UserId).Select(z => z.TrainingHours).DefaultIfEmpty(0).Sum(),
                    Successes = AgentLogs.Where(y => y.AgentId == x.UserId).Select(c => c.Successes).DefaultIfEmpty(0).Sum()
                });
                AgentProductionModel = AgentProductionModel.Select(c => { c.Penalty = Penalties.FirstOrDefault(x => x.From < c.HourPercentage && x.To > c.HourPercentage && !x.IsDeleted && x.PayRate == c.UserCampaign.PayRateDialingHours)?.PenaltyFee ?? 0; return c; });
                var CampaignsSelectList = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                CampaignsSelectList.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(CampaignsSelectList, "Id", "Identifier", model.CampaignId);
                return View(AgentProductionModel);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "AgentProduction", AlertType.error, ex);
                return View(new AgentProductionViewModel());
            }
        }

        public async Task<ActionResult> OnWatch()
        {
            try
            {
                var AgentLogs = await db.AgentLogs.Where(x => !x.IsDeleted).ToListAsync();
                var Campaigns = await db.Campaigns.Where(x => !x.IsDeleted).ToListAsync();
                var OnWatchSettings = await db.OnWatchSettings.ToListAsync();

                var onWatchModel = Campaigns.Select(x => new OnWatchViewModel
                {
                    IsActive = OnWatchSettings.Where(d => d.CampaignId == x.Id).Count() > 0,
                    HoursLeft = OnWatchSettings.FirstOrDefault(d => d.CampaignId == x.Id)?.HoursLeft,
                    CampaignLimit = x.CampaignLimit,
                    Campaign = x,
                    CampaignId = x.Id,
                    TotalHours = AgentLogs.Where(m => m.CampaignId == x.Id).Select(k => k.DialingHours).DefaultIfEmpty(0).Sum()
                }).ToList();

                return View(onWatchModel);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "OnWatch", AlertType.error, ex);
                return View(new OnWatchViewModel());
            }

        }

        [HttpPost]
        public async Task<ActionResult> OnWatch(List<OnWatchViewModel> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var OnWatchSettings = await db.OnWatchSettings.ToListAsync();
                    db.OnWatchSettings.RemoveRange(OnWatchSettings);
                    var modelAdd = new List<OnWatchSettings>();

                    foreach (var item in model)
                    {
                        if (item.IsActive)
                        {
                            modelAdd.Add(new OnWatchSettings() { CampaignId = item.CampaignId, HoursLeft = item.HoursLeft ?? 0, CreationDate = DateTime.Now, LastUpdate = DateTime.Now });
                        }
                    }

                    db.OnWatchSettings.AddRange(modelAdd);
                    await db.SaveChangesAsync();
                    return RedirectToAction("OnWatch");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "OnWatchPost", AlertType.error, ex);
                return View(new OnWatchViewModel());
            }
        }

        public class DataInventoryRequest
        {
            public List<Record> Records { set; get; }
            public List<Campaign> Campaigns { set; get; }
            public List<Appointments> Callbacks { set; get; }
        }
        public class DataInventoryByTimeZoneRequest
        {
            public List<Entities.TimeZone> TimeZones { set; get; }
            public List<Record> Records { set; get; }
            public List<Campaign> Campaigns { set; get; }
            public Dictionary<int, Dictionary<int, int>> Callbacks { set; get; }
        }


        public async Task<DataInventoryRequest> DataInventoryLoad(DateTime startDate, DateTime endDate)
        {
            await db.Users
                .Where(x => !x.IsDeleted
                    && x.Roles.Any(role => role.RoleId == "2"))
                .LoadAsync();

            await db.Records
                .Where(x => !x.Contact.IsDeleted
                    && !x.IsDeleted
                    && x.StartTime >= startDate
                    && x.EndTime <= endDate)
                .Select(x => x.Contact)
                .LoadAsync();

            await db.Projects
                .Where(x => !x.IsDeleted)
                .LoadAsync();

            var output = new DataInventoryRequest
            {
                Records = await db.Records
                    .Where(x => !x.IsDeleted
                        && x.StartTime >= startDate
                        && x.EndTime <= endDate)
                    .ToListAsync(),

                Campaigns = await db.Campaigns
                    .Where(x => !x.IsDeleted)
                    .ToListAsync(),

                Callbacks = await db.Appointments
                    .Where(x => !x.IsDeleted
                        && x.IsScheduled
                        && x.Record != null)
                    .ToListAsync(),
            };

            return output;
        }
        public async Task<DataInventoryByTimeZoneRequest> DataInventoryByTimeZoneLoad()
        {
            await db.Projects
                .Where(x => !x.IsDeleted)
                .LoadAsync();

            var groups = await db.Appointments
                .Where(x => !x.IsDeleted
                    && x.IsScheduled
                    && x.Record != null)
                .GroupBy(x => new { x.Record.Contact.TimeZoneId, x.Record.Contact.ProjectId })
                .Select(x => new DTO.DataInventoryByTimeZone.Callback
                {
                    TimeZoneId = x.Key.TimeZoneId,
                    ProjectId = x.Key.ProjectId,
                    Total = x.Count()
                })
                .ToListAsync();

            var callbacks = new Dictionary<int, Dictionary<int, int>>();
            foreach (var group in groups)
            {
                if (!callbacks.ContainsKey(group.ProjectId))
                    callbacks.Add(group.ProjectId, new Dictionary<int, int>());

                if (!callbacks[group.ProjectId].ContainsKey(group.TimeZoneId))
                    callbacks[group.ProjectId].Add(group.TimeZoneId, group.Total);

            }

            var output = new DataInventoryByTimeZoneRequest
            {
                Callbacks = callbacks,

                Campaigns = await db.Campaigns
                    .Where(x => !x.IsDeleted)
                    .ToListAsync(),

                TimeZones = await db.TimeZones
                    .Where(x => !x.IsDeleted)
                    .ToListAsync(),
            };

            return output;
        }
    }
}