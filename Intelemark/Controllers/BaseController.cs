using Intelemark.Entities;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Intelemark.Controllers
{
    public class BaseController : Controller
    {
        private static readonly log4net.ILog log
       = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void AddAlert(string message, AlertType type, PositionClass positionClass = PositionClass.TopRight, string Timeout = "5000")
        {
            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            if (NotificationHub.Connections.TryGetValue(User.Identity.Name, out ConnectionHandler connId))
                foreach (string s in connId.ConnectionId)
                    notificationHub.Clients.Client(s).displayStatus(message, type.ToString(), Alert.GetStringValue(positionClass));
        }
      
        public void AddAlert(string message, string CallerMethod, string CallerClass, AlertType type,  Exception ex, PositionClass positionClass = PositionClass.TopRight, string Timeout = "5000")
        {
            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            if (NotificationHub.Connections.TryGetValue(User.Identity.Name, out ConnectionHandler connId))
                foreach (string s in connId.ConnectionId)
                    notificationHub.Clients.Client(s).displayStatus(message, type.ToString(), Alert.GetStringValue(positionClass));

            log.Error($"[{CallerMethod}] {ex.Message}" , ex);
        }

        public void AddAlertToUser(string message, string user, AlertType type, PositionClass positionClass = PositionClass.TopRight, string Timeout = "5000")
        {
            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            if (NotificationHub.Connections.TryGetValue(user, out ConnectionHandler connId))
                foreach (string s in connId.ConnectionId)
                    notificationHub.Clients.Client(s).displayStatus(message, type.ToString(), Alert.GetStringValue(positionClass), Timeout);
        }

        public void AddSwal(string message)
        {
            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            if (NotificationHub.Connections.TryGetValue(User.Identity.Name, out ConnectionHandler connId))
                foreach (string s in connId.ConnectionId)
                    notificationHub.Clients.Client(s).sweetAlert(message);
        }

        public static void LogError(string message, string CallerMethod, Exception ex)
        {
            log.Error($"({CallerMethod}) {ex.Message}", ex);
        }

    }

    

}