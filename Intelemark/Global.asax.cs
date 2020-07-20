using Intelemark.ScriptServices;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Intelemark
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //new FixIntelemark().Run();
            // RegisterNotification();
        }

        //private void RegisterNotification()
        //{
        //    //Get the connection string from the Web.Config file. Make sure that the key exists and it is the connection string for the Notification Database and the NotificationList Table that we created
        //    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        //    //We have selected the entire table as the command, so SQL Server executes this script and sees if there is a change in the result, raise the event
        //    string commandText = @"
        //                            Select
        //                                dbo.Notifications.Id
        //                            From
        //                                dbo.Notifications
        //    Where
        //		dbo.Notifications.Notified = 0  AND dbo.Notifications.ScheduledDate =                               
        //                            ";

        //    //Start the SQL Dependency
        //    SqlDependency.Start(connectionString);
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {

        //        using (SqlCommand command = new SqlCommand(commandText, connection))
        //        {
        //            connection.Open();
        //            var sqlDependency = new SqlDependency(command);


        //            sqlDependency.OnChange += new OnChangeEventHandler(sqlDependency_OnChange);

        //            // NOTE: You have to execute the command, or the notification will never fire.
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //            }
        //        }
        //    }
        //}

        //private void sqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        //{

        //    if (e.Info == SqlNotificationInfo.Insert)
        //    {

        //        var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

        //        ApplicationDbContext db = new ApplicationDbContext();

        //        var notifications = db.Notification.Where(x => x.ScheduledDate <= DateTime.Now && !x.Notified).ToList();

        //        foreach (var item in notifications)
        //        {
        //            var message = item.Appointment?.Record == null ? item.Appointment?.Notes : $"You have an upcoming callback with {item.Appointment.Record.Contact} at {item.Appointment.DateScheduled}";
        //            if (NotificationHub.Connections.TryGetValue(item.User.Email, out ConnectionHandler connId))
        //                foreach (string s in connId.ConnectionId)
        //                    notificationHub.Clients.Client(s).displayStatus(message, AlertType.info, Alert.GetStringValue(PositionClass.TopFullWidth));
        //            item.Notified = true;
        //            db.Entry(item).State = EntityState.Modified;
        //        }

        //    }
        //    //Call the RegisterNotification method again
        //    RegisterNotification();
        //}
    }
}
