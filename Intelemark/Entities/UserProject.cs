using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class UserProject : Entity
    {
        public String UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Int32 ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public Int32 UserCampaignId { get; set; }
        public virtual UserCampaign UserCampaign { get; set; }
    }
}


