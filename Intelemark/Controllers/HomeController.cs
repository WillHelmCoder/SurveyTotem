using Intelemark.Entities;
using Intelemark.Models;
using Intelemark.Models.Reports;
using Intelemark.ScriptServices;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Intelemark.Controllers
{
    [ExtensionMethods.Extensions.CheckSession]

    [Authorize]
    public class HomeController : BaseController
    {
        //[AllowAnonymous]
        //public async Task<ActionResult> fex()
        //{
        //    var script = new MigrationScript();
            
        //    await script.Run();

        //    return HttpNotFound();
        //}

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(DailyActivityReportViewModel model)
        {
            try
            {
                model.CampaignId = -1;
                model.StartDate = DateTime.Now.AddDays(-1);
                model.EndDate = DateTime.Now;

                var recordsQuery = db.Records
                    .Where(x => !x.IsDeleted
                        && (x.Form.CampaignId == model.CampaignId || model.CampaignId == -1) );

                var contactsQuery = db.Contacts
                    .Where(x => !x.IsDeleted
                        && (x.Project.CampaignId == model.CampaignId || model.CampaignId == -1));

                var totalSeconds = recordsQuery.Any() ?
                    recordsQuery.Sum(x => x.Duration) : 0;

                var totalHours = decimal.Round((decimal)totalSeconds / 3600, 2);

                var report = new DailyActivityReportViewModel
                {
                    TotalHours = totalHours.ToString(),
                    TotalSuccesses = recordsQuery.Where(x => x.CallCode.IsSuccess).Count(),
                    TotalDials = recordsQuery.Count(),
                    ContactsCompleted = recordsQuery.Where(x => x.Contact.IsFinalized).Count(),
                    ContactsInCallBack = recordsQuery.Where(x => x.CallCode.Behavior == Utilities.EOCBehaviors.CallBack).Count(),
                    ContactsInHold = recordsQuery.Where(x => x.CallCode.Behavior == Utilities.EOCBehaviors.FinalizeRecord).Count(),
                    DialsPerSuccess = recordsQuery.Where(x => x.CallCode.IsSuccess).Count() == 0 ? 0 : recordsQuery.Count() / recordsQuery.Where(x => x.CallCode.IsSuccess).Count(),
                    TotalContacts = contactsQuery.Count(),
                    ContactRemaining = contactsQuery.Where(x => x.AgentId == null).Count(),


                    CallCodes = db.Records.Where(x => x.Form.CampaignId == model.CampaignId || model.CampaignId == -1).GroupBy(info => info.CallCode)
                           .Select(group => new CallCodeModel
                           {
                               Code = group.Key.Code,
                               Name = group.Key.Name,
                               Count = group.Count()
                           }).ToList(),

                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now
                };

                var Campaigns = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                Campaigns.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(Campaigns, "Id", "Identifier");
                return View(report);

            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "Index-Dashboard", AlertType.error, ex);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(BaseReportModel model)
        {
            try
            {
                var recordsQuery = db.Records.Where(x => (x.Form.CampaignId == model.CampaignId || model.CampaignId == -1) && x.CreationDate >= model.StartDate && x.CreationDate <= model.EndDate && !x.IsDeleted);
                var contactsQuery = db.Contacts.Where(x => (x.Project.CampaignId == model.CampaignId || model.CampaignId == -1) && x.CreationDate >= model.StartDate && x.CreationDate <= model.EndDate && !x.IsDeleted);

                var totalSeconds = recordsQuery
                    .Sum(x => x.Duration);

                var totalHours = decimal.Round((decimal)totalSeconds / 3600, 2);

                var report = new DailyActivityReportViewModel
                {
                    TotalHours = totalHours.ToString(),
                    TotalSuccesses = recordsQuery.Where(x => x.CallCode.IsSuccess).Count(),
                    TotalDials = recordsQuery.Count(),
                    ContactsCompleted = recordsQuery.Where(x => x.Contact.IsFinalized).Count(),
                    ContactsInCallBack = recordsQuery.Where(x => x.CallCode.Behavior == Utilities.EOCBehaviors.CallBack).Count(),
                    ContactsInHold = recordsQuery.Where(x => x.CallCode.Behavior == Utilities.EOCBehaviors.FinalizeRecord).Count(),
                    DialsPerSuccess = recordsQuery.Where(x => x.CallCode.IsSuccess).Count() == 0 ? 0 : recordsQuery.Count() / recordsQuery.Where(x => x.CallCode.IsSuccess).Count(),
                    TotalContacts = contactsQuery.Count(),
                    ContactRemaining = contactsQuery.Where(x => x.AgentId == null).Count(),

                    CallCodes = db.Records.Where(x => x.Form.CampaignId == model.CampaignId || model.CampaignId == -1).GroupBy(info => info.CallCode)
                           .Select(group => new CallCodeModel
                           {
                               Code = group.Key.Code,
                               Name = group.Key.Name,
                               Count = group.Count()
                           }).ToList(),

                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };

                var Campaigns = db.Campaigns.Where(x => !x.IsDeleted).ToList();
                Campaigns.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(Campaigns, "Id", "Identifier", model.CampaignId);
                return View(report);

            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "Index-Dashboard", AlertType.error, ex);
                return View();
            }
        }

        public ActionResult Calendar()
        {
            ViewBag.UserId = new SelectList(db.Users.Where(x => !x.IsDeleted), "Id", "DisplayName");
            ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SaveAppointment(AppointmentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = new Appointments
                    {
                        AgentId = model.AgentId,
                        Notes = model.Notes,
                        DateScheduled = model.DateScheduled,
                        CampaignId = model.CampaignId,
                        IsScheduled = false,
                        IsConfirmed = false,
                        TimesScheduled = 0,
                        LastUpdate = DateTime.Now,
                        CreationDate = DateTime.Now,
                        ReminderDate = model.DateScheduled.AddMinutes(-10),
                        IsDeleted = false,
                    };
                    db.Appointments.Add(entity);
                    await db.SaveChangesAsync();
                    return Json("Ok");
                }
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create(model)", this.GetType().ToString(), AlertType.error, e);
            }

            ViewBag.UserId = new SelectList(db.Users.Where(x => !x.IsDeleted), "Id", "DisplayName");
            ViewBag.CampaignId = new SelectList(db.Campaigns.Where(x => !x.IsDeleted), "Id", "Identifier");
            return Json("Error");
        }

        [HttpPost]
        public async Task<JsonResult> RescheduleAppointment(AppointmentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await db.Appointments.FindAsync(model.Id);
                    entity.DateScheduled = model.DateScheduled;
                    entity.Notes = model.Notes;
                    entity.TimesScheduled = entity.TimesScheduled + 1;
                    entity.LastUpdate = DateTime.Now;

                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Json("Ok");
                }
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create(model)", this.GetType().ToString(), AlertType.error, e);
                return Json("Error");
            }
            return Json("Error");
        }

        [HttpPost]
        public async Task<JsonResult> ConfirmAppointment(Int32 Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await db.Appointments.FindAsync(Id);
                    entity.IsConfirmed = true;
                    entity.LastUpdate = DateTime.Now;
                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Json("Ok");
                }
                return Json("Error");
            }
            catch (Exception e)
            {
                return Json("Error");
            }
        }

        [HttpPost]
        public async Task<JsonResult> CancelAppointment(Int32 Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await db.Appointments.FindAsync(Id);

                    entity.IsConfirmed = false;
                    entity.LastUpdate = DateTime.Now;
                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Json("Ok");
                }
                return Json("Error");
            }
            catch (Exception e)
            {
                return Json("Error");
            }
        }

        [HttpGet]
        public JsonResult GetAppointments()
        {
            try
            {
                String UrlAction = User.IsInRole("Agent") ? "" : $"javascript:ReSchedule([Id]);";
                var CampaignId = User.IsInRole("Admin") ? -1 : (Int32)Session["CampaignId"];
                List<events> events = db.Appointments.AsEnumerable<Appointments>().Where(x => (x.AgentId == User.Identity.GetUserId() || User.IsInRole("Admin") || User.IsInRole("Account Manager")) && (x.CampaignId == CampaignId || CampaignId == -1)).Select(x =>
                new events
                {
                    title = $"{x.Notes}",
                    start = x.DateScheduled.ToString("yyyy-MM-ddThh:mm"),
                    color = x.IsConfirmed ? "#37BC9B" : "#DA4453",
                    url = x.IsConfirmed ? UrlAction.Replace("[Id]", x.Id.ToString()) : $"javascript:Confirm({x.Id});",
                }).ToList();

                return Json(events, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Create(model)", this.GetType().ToString(), AlertType.error, e);
            }

            return Json("Error");
        }


        public async Task<ActionResult> ScheduleSummary()
        {
            try
            {
                var StartDate = DateTime.Now.AddDays(-7);
                var EndDate = DateTime.Now.AddDays(1);

                var model = await db.Appointments.Where(x => !x.IsDeleted).Select(x => new AppointmentModel
                {
                    Notes = x.Notes,
                    IsConfirmed = x.IsConfirmed,
                    TimesScheduled = x.TimesScheduled,
                    DateScheduled = x.DateScheduled,
                    Id = x.Id,
                    Agent = x.Agent,
                    Campaign = x.Campaign,
                    Record = x.Record,
                    CreationDate = x.CreationDate,
                    LastUpdate = x.LastUpdate,
                    StartDate = StartDate,
                    EndDate = EndDate
                }
                ).ToListAsync();

                var Campaigns = await db.Campaigns.Where(x => !x.IsDeleted).ToListAsync();
                Campaigns.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(Campaigns, "Id", "Identifier");
                return View(model);

            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "Index-Dashboard", AlertType.error, ex);
                var Campaigns = await db.Campaigns.Where(x => !x.IsDeleted).ToListAsync();
                Campaigns.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(Campaigns, "Id", "Identifier");
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> ScheduleSummary(AppointmentModel appointment)
        {
            try
            {
                var StartDate = DateTime.Now.AddDays(-7);
                var EndDate = DateTime.Now.AddDays(1);

                var model = await db.Appointments.Where(x => !x.IsDeleted && x.DateScheduled >= appointment.StartDate && x.DateScheduled <= appointment.EndDate && (x.CampaignId == appointment.CampaignId || appointment.CampaignId == -1)).Select(x => new AppointmentModel
                {
                    Notes = x.Notes,
                    IsConfirmed = x.IsConfirmed,
                    TimesScheduled = x.TimesScheduled,
                    DateScheduled = x.DateScheduled,
                    Id = x.Id,
                    Agent = x.Agent,
                    Campaign = x.Campaign,
                    Record = x.Record,
                    CreationDate = x.CreationDate,
                    LastUpdate = x.LastUpdate,
                    StartDate = StartDate,
                    EndDate = EndDate
                }
                ).ToListAsync();

                var Campaigns = await db.Campaigns.Where(x => !x.IsDeleted).ToListAsync();
                Campaigns.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(Campaigns, "Id", "Identifier", appointment.CampaignId);
                return View(model);

            }
            catch (Exception ex)
            {
                AddAlert("Oops, Something Happened!", this.GetType().ToString(), "Index-Dashboard", AlertType.error, ex);
                var Campaigns = await db.Campaigns.Where(x => !x.IsDeleted).ToListAsync();
                Campaigns.Insert(0, new Campaign { Id = -1, Identifier = "All Campaigns" });
                ViewBag.CampaignId = new SelectList(Campaigns, "Id", "Identifier");
                return View();
            }
        }


        public async Task<ActionResult> AlertSettings()
        {
            var model = await db.AlertSettings.Select(x => new AlertSettingsModel { Id = x.Id, NotificationEmails = x.NotificationEmails, DataPercentage = x.DataPercentage }).FirstOrDefaultAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AlertSettings(AlertSettingsModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await db.AlertSettings.FindAsync(model.Id);

                    if (entity == null)
                        return HttpNotFound();

                    entity.NotificationEmails = model.NotificationEmails;
                    entity.DataPercentage = model.DataPercentage;
                    entity.LastUpdate = DateTime.Now;

                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception e)
            {
                AddAlert($"Oops! something went wrong. Error code: {e.HResult}", "Edit(model)", this.GetType().ToString(), AlertType.error, e);
                return View(model);
            }
        }

        public class events
        {
            public String title { get; set; }
            public String start { get; set; }
            public String color { get; set; }
            public String url { get; set; }
        }

    }
}