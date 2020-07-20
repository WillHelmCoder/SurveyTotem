using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class UserCampaign : Entity
    {
        public String UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Int32 CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        public Double PayRateTrainingHours { get; set; }
        public Double PayRateDialingHours { get; set; }
        public Double PayRateSuccess { get; set; }
        public Double BudgetedHours { get; set; }

        public virtual List<UserProject> UserProjectList { get; set;}
    }
}


