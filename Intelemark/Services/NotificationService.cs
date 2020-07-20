using Intelemark.Controllers;
using Intelemark.Entities;
using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGrease.Css.Extensions;
using System.Threading.Tasks;
using System.Data.Entity;
using RestSharp;
using RestSharp.Authenticators;
using Intelemark.Models.Reports;

namespace Intelemark.Services
{
    public class NotificationService : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Execute()
        {
            var notifications = AvailableAppointments().Result;
            notifications.ForEach(x =>
            {
                var message = x.IsScheduled ? $"Hi! Just a reminder that you have a call scheduled with {x.Record.Contact.Name} coming up at {x.DateScheduled.ToShortTimeString()}" : $"You have an appointment scheduled at {x.DateScheduled.ToShortTimeString()}";
                AddAlertToUser(message, x.Agent.Email, AlertType.info, PositionClass.TopFullWidth, "0");
                x.Notified = true;
                db.Entry(x).State = EntityState.Modified;
            });
            db.SaveChanges();
        }

        public async Task CheckForDaylight()
        {
            var daylight = System.TimeZone.CurrentTimeZone.GetDaylightChanges(DateTime.Now.Year);
            if (DateTime.Now >= daylight.Start && DateTime.Now <= daylight.End)
            {
                var entities = await db.TimeZones.ToListAsync();
                foreach (var item in entities)
                {
                    item.CurrentTime = item.DST;
                    db.Entry(item).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
            }
            else
            {
                var entities = await db.TimeZones.ToListAsync();
                foreach (var item in entities)
                {
                    item.CurrentTime = item.DST;
                    db.Entry(item).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
            }

        }

        private async Task<List<Appointments>> AvailableAppointments()
        {
            List<Appointments> notifications = null;
            var DateToCheck = DateTime.Now.ToUniversalTime();
            notifications = await db.Appointments.Where(x => x.ReminderDate <= DateToCheck && x.Notified == false).ToListAsync();
            return notifications;
        }

        public async Task LowDataNotification()
        {
            //LOW DATA SETTINGS NOTIFICATIONS
            var LowDataSettings = await db.AlertSettings.FirstOrDefaultAsync();
            var Projects = await db.Contacts.GroupBy(x => x.Project).Select(g => new { Project = g.Key, TotalRecords = g.Count(), ActiveRecords = g.Sum(c => c.IsFinalized ? 0 : 1) }).ToListAsync();
            Projects = Projects.Where(x => x.ActiveRecords > 0 && GetDataPercentage(x.ActiveRecords, x.TotalRecords) <= LowDataSettings.DataPercentage).ToList();
            if (LowDataSettings != null || Projects != null || Projects.Count > 0)
                foreach (var item in Projects)
                    SendEmail(item.Project, LowDataSettings, GetDataPercentage(item.ActiveRecords, item.TotalRecords));

            //CAMPAIGN LIMIT HOURS NOTIFICATIONS
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
            }).Where(x => x.TotalHours >= (x.CampaignLimit - x.HoursLeft)).ToList();

            if (onWatchModel.Count() > 0)
                SendCampaignNotifications(onWatchModel, LowDataSettings);
        }

        public double GetDataPercentage(double ActiveRecords, double TotalRecords)
        {
            return (ActiveRecords / TotalRecords) * 100;
        }

        public bool SendEmail(Project project, AlertSettings Alert, double DataPercentage)
        {
            try
            {
                string body = "<table cellspacing=\"1\" class=\"x_m_-3435250125640658816style1\">" +
                               "    <tbody>" +
                               "        <tr>" +
                               "            <td align=\"center\" bgcolor=\"#EFEFEF\" colspan=\"2\"><img data-imagetype=\"External\" src=\"/wp-content/themes/intelemark2/images/intelemark-logo.png\" width=\"200\"> </td>" +
                               "        </tr>" +
                               "        <tr>" +
                               $"            <td>The project {project.Name}'s data has fallen below the specified percentage</td>" +
                               "        </tr>" +
                               "        <tr>" +
                               "            <td bgcolor=\"#EFEFEF\">Current active percentage</td>" +
                               $"            <td bgcolor=\"#EFEFEF\">{DataPercentage}%</td>" +
                               "        </tr>" +
                               "        <tr>" +
                               "            <td>Percentage limit</td>" +
                               $"            <td>{Alert.DataPercentage}%</td>" +
                               "        </tr>" +
                               "        <tr>" +
                               "            <td bgcolor=\"#EFEFEF\">Campaign</td>" +
                               $"            <td bgcolor=\"#EFEFEF\">{project.Campaign.Identifier}</td>" +
                               "        </tr>" +
                               "        <tr>" +
                               "            <td align=\"center\" bgcolor=\"#f4a201\" colspan=\"2\"><b><font color=\"white\">Intelemark Web Application</font></b></td>" +
                               "        </tr>" +
                               "    </tbody>" +
                               "</table>";

                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator =
                new HttpBasicAuthenticator("api",
                                          "key-9b7b33bcc4285cc7337a96e3609a61ec");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "mails.inovercy.com", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "<notifications@xipetechnology.com>");
                request.AddParameter("to", Alert.NotificationEmails.Replace(',', ';'));
                request.AddParameter("subject", "Low data notification");
                request.AddParameter("html", body);
                request.Method = Method.POST;
                var send = client.Execute(request);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendCampaignNotifications(List<OnWatchViewModel> onWatchSettings, AlertSettings Alert)
        {
            try
            {
                string body = "<table cellspacing=\"1\" class=\"x_m_-3435250125640658816style1\">" +
                               "    <tbody>" +
                               "        <tr>" +
                               "            <td align=\"center\" bgcolor=\"#EFEFEF\" colspan=\"3\"><img data-imagetype=\"External\" src=\"/wp-content/themes/intelemark2/images/intelemark-logo.png\" width=\"300\"> </td>" +
                               "        </tr>" +
                               "        <tr>" +
                               $"            <td colspan=\"3\">The following campaign(s) working hours have reached the notification point:</td>" +
                               "        </tr>" +
                               "        <tr>" +
                               $"            <td><b>Campaign</b></td>" +
                               $"            <td><b>Campaign Limit</b></td>" +
                               $"            <td><b>Hours Left</b></td>" +
                               "        </tr>";

                foreach (var item in onWatchSettings)
                {
                    body += " <tr>" +
                  $"            <td bgcolor=\"#EFEFEF\">{item.Campaign.Identifier}</td>" +
                  $"            <td bgcolor=\"#EFEFEF\">{item.Campaign.CampaignLimit}</td>" +
                  $"            <td bgcolor=\"#EFEFEF\">{item.HoursLeft}</td>" +
                  "        </tr>";
                }

                body += "<tr>" +
                "            <td align=\"center\" bgcolor=\"#f4a201\" colspan=\"3\"><b><font color=\"white\">Intelemark Web Application</font></b></td>" +
                "        </tr>" +
                "    </tbody>" +
                "</table>";

                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator =
                new HttpBasicAuthenticator("api",
                                          "key-9b7b33bcc4285cc7337a96e3609a61ec");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "mails.inovercy.com", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "<notifications@xipetechnology.com>");
                request.AddParameter("to", Alert.NotificationEmails.Replace(',', ';'));
                request.AddParameter("subject", "Campaign hour limit notification");
                request.AddParameter("html", body);
                request.Method = Method.POST;
                var send = client.Execute(request);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
