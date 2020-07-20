using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class UserHistory : Entity
    {
        public String UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Campaign Campaign { get; set; }
        public Int32? CampaignId { get; set; }
        public DateTime LoggedIn { get; set; }
        public DateTime LoggedOff { get; set; }
        public double? Duration { get; set; }
        public String ConnectionId { get; set; }

    }
}



