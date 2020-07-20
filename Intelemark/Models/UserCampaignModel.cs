using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Intelemark.Entities;
using Intelemark.Utilities;

namespace Intelemark.Models
{
    public class UserCampaignModel : Model
    {
        [Required]
        [Display(Name = "User")]
        public String UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Display(Name = "Campaign")]
        public Int32 CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        [Required]
        [Display(Name = "Pay Rate Training Hours")]
        public Double PayRateTrainingHours { get; set; }

        [Required]
        [Display(Name = "Pay Rate Dialing Hours")]
        public Double PayRateDialingHours { get; set; }

        [Required]
        [Display(Name = "Pay Rate Success")]
        public Double PayRateSuccess { get; set; }

        [Required]
        [Display(Name = "Budgeted Hours")]
        public Double BudgetedHours { get; set; }

        [Required]
        [Display(Name = "Projects")]
        public List<Int32> ProjectIds { get; set; }

        public Int32? confirmOverwite { get; set;  }

        public List<UserProjectModel> UserProjectsList { get; set; }
        public virtual UserProjectModel UserProjects { get; set; }
    }

}