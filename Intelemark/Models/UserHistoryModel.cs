using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class UserHistoryModel : Model
    {
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime LoggedIn { get; set; }
        public DateTime LoggedOff { get; set; }
        public double? Duration { get; set; }
        public String ConnectionId { get; set; }

        public List<OnlineUser> OnlineUsers { get; set; }
        public List<ApplicationUser> OfflineUsers { get; set; }
    }

    public class OnlineUser
    {
        public ApplicationUser User { get; set; }
        public Campaign Campaign { get; set; }
        public TimeSpan ConnectionTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.##} Minutes")]
        public Double TotalTime { get => ConnectionTime.TotalMinutes; }

    }

}