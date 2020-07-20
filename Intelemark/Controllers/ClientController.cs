using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Intelemark.Entities;
using Intelemark.Models;
using Intelemark.Models.Reports;
using Intelemark.Services;
using Microsoft.AspNet.Identity;

namespace Intelemark.Controllers
{
    [Authorize]
    public class ClientController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public async Task<ActionResult> Index(DailyActivityReportViewModel model)
        {
            try
            {
                var clientId = User.Identity.GetUserId();
                var Campaigns = await db.Campaigns.Where(x => x.ClientId == clientId && !x.IsDeleted).ToListAsync();
                var CampaignsInts = Campaigns.Select(z => z.Id);

                model.StartDate = DateTime.Now.AddDays(-1);
                model.EndDate = DateTime.Now;

                var recordsQuery = await db.Records.Where(x => CampaignsInts.Contains(x.Form.CampaignId) && !x.IsDeleted).ToListAsync();
                var contactsQuery = await db.Contacts.Where(x => CampaignsInts.Contains(x.Project.Campaign.Id) && !x.IsDeleted).ToListAsync();

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


                    CallCodes = recordsQuery.GroupBy(info => info.CallCode)
                           .Select(group => new CallCodeModel
                           {
                               Code = group.Key.Code,
                               Name = group.Key.Name,
                               Count = group.Count()
                           }).ToList(),

                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now
                };


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
        public async Task<ActionResult> Index(BaseReportModel model)
        {
            try
            {
                var clientId = User.Identity.GetUserId();
                var Campaigns = await db.Campaigns.Where(x => x.ClientId == clientId && !x.IsDeleted).ToListAsync();
                var CampaignsInts = Campaigns.Select(z => z.Id);
                var recordsQuery = new List<Record>();
                var contactsQuery = new List<Contact>(); ;

                if (model.CampaignId == -1)
                {
                    recordsQuery = await db.Records.Where(x => CampaignsInts.Contains(x.Form.CampaignId) && x.CreationDate >= model.StartDate && x.CreationDate <= model.EndDate && !x.IsDeleted).ToListAsync();
                    contactsQuery = await db.Contacts.Where(x => CampaignsInts.Contains(x.Project.Campaign.Id) && x.CreationDate >= model.StartDate && x.CreationDate <= model.EndDate && !x.IsDeleted).ToListAsync();
                }
                else
                {
                    recordsQuery = await db.Records.Where(x => (x.Form.CampaignId == model.CampaignId) && x.CreationDate >= model.StartDate && x.CreationDate <= model.EndDate && !x.IsDeleted).ToListAsync();
                    contactsQuery = await db.Contacts.Where(x => (x.Project.CampaignId == model.CampaignId) && x.CreationDate >= model.StartDate && x.CreationDate <= model.EndDate && !x.IsDeleted).ToListAsync();
                }

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

                CallCodes = recordsQuery.GroupBy(info => info.CallCode)
                       .Select(group => new CallCodeModel
                       {
                           Code = group.Key.Code,
                           Name = group.Key.Name,
                           Count = group.Count()
                       }).ToList(),

                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };

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
    }
}
