
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Intelemark.Entities;
using Intelemark.Models;
using Microsoft.AspNet.SignalR;

namespace Intelemark
{
    public class ConnectionHandler
    {

        public int UserHistoryId { get; set; }
        public int? CampaignId { get; set; }
        public HashSet<String> ConnectionId { get; set; }
        public String UserName { get; set; }
    }


    public class NotificationHub : Hub
    {
        public static ConcurrentDictionary<string, ConnectionHandler> Connections = new ConcurrentDictionary<string, ConnectionHandler>();
        private readonly ApplicationDbContext DbContext = new ApplicationDbContext();

        public override Task OnConnected()
        {
            var name = Context.User.Identity.Name;
            var connectionID = Context.ConnectionId;
            var userHistoryId = 0;
            if (!Connections.ContainsKey(Context.User.Identity.Name))
            {
                var User = DbContext.Users.FirstOrDefault(x => x.Email.Equals(name));

                if (User == null)
                    return base.OnConnected();

                var newUserHistory = DbContext.UsersHistory.Add(new UserHistory()
                {
                    User = User,
                    UserId = User.Id,
                    LoggedIn = DateTime.Now,
                    LoggedOff = DateTime.Now,
                    ConnectionId = connectionID,
                    LastUpdate = DateTime.Now,
                    CreationDate = DateTime.Now,
                    IsDeleted = false
                });
                DbContext.SaveChanges();
                userHistoryId = newUserHistory.Id;
            }

            var newConnection = Connections.GetOrAdd(name, new ConnectionHandler() {UserName = name, UserHistoryId = userHistoryId, CampaignId = null,  ConnectionId = new HashSet<string>() });
            try
            {
                newConnection.ConnectionId.Add(connectionID);
            }
            catch
            {

            }


            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (!Connections.TryGetValue(Context.User.Identity.Name, out ConnectionHandler toQueue))
                return base.OnDisconnected(stopCalled); ;

            Task mytask = Task.Run(() =>
            {
                UserDisconnected(Context.User.Identity.Name, Context.ConnectionId);
            });

            return base.OnDisconnected(stopCalled);
        }

        private async void UserDisconnected(string name, string connId)
        {
            await Task.Delay(10000);
            if (Connections.TryGetValue(name, out ConnectionHandler user))
            {
                user.ConnectionId.Remove(connId);
                if (user.ConnectionId.Count <= 0)
                {

                    if (Context.User.IsInRole("Agent") || Context.User.IsInRole("Account Manager"))
                    {
                        var myId = Context.User.Identity.GetUserId();
                        var contacts = await DbContext.Contacts.Where(x => x.OnDial && x.AgentId == myId).ToListAsync();

                        foreach (var item in contacts)
                        {
                            item.AgentId = null;
                            item.OnDial = false;
                            DbContext.Entry(item).State = EntityState.Modified;
                        }
                        await DbContext.SaveChangesAsync();
                    }

                    var entity = DbContext.UsersHistory.FirstOrDefault(x => x.Id == user.UserHistoryId);
                    entity.LoggedOff = DateTime.Now;
                    entity.Duration = (DateTime.Now - entity.LoggedIn).TotalSeconds;
                    entity.CampaignId = user.CampaignId;
                    DbContext.Entry(entity).State = EntityState.Modified;
                    await DbContext.SaveChangesAsync();
                    Connections.TryRemove(name, out user);
                }
            }

        }

        public void SendAlert(string message, string style, string alignment)
        {
            if (Connections.TryGetValue(Context.User.Identity.Name, out ConnectionHandler connId))
                foreach(string s in connId.ConnectionId)
                    this.Clients.Client(s).displayStatus(message, style, alignment);
        }

        public static List<String> OnlineAgents()
        {
           return Connections.Keys.ToList();
        }

        public static List<ConnectionHandler> GetConnections()
        {
            return Connections.Values.ToList();
        }

        public void SendCampaign(int campaignId)
        {
            if (Connections.TryGetValue(Context.User.Identity.Name, out ConnectionHandler connId))
            {
                if (connId.CampaignId == null)
                {
                    connId.CampaignId = campaignId;
                }
            }
        }

    }
}