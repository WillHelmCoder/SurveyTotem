using Hangfire;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intelemark.Services
{
    public class Hangfire
    {
        public static void ConfigureHangfire(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("DefaultConnection");

            app.UseHangfireDashboard("/jobs");
            app.UseHangfireServer();
        }

        public static void InitializeJobs()
        {
            //RecurringJob.AddOrUpdate<NotificationService>(x => x.Execute(), Cron.Minutely);
            //RecurringJob.AddOrUpdate<NotificationService>(x => x.CheckForDaylight(), Cron.Daily);
            //RecurringJob.AddOrUpdate<NotificationService>(x => x.LowDataNotification(), Cron.Daily);
        }
    }
}