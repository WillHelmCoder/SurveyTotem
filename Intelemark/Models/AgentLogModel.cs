using Intelemark.Entities;
using Intelemark.Models;
using Intelemark.Models.Reports;
using Intelemark.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class AgentLogModel : BaseReportAgentModel
    {
        [Display(Name = "Dialing Hours")]
        [Required]
        public Double DialingHours { get; set; }

        [Display(Name = "Training Hours")]
        [Required]
        public Double TrainingHours { get; set; }

        [Display(Name = "Succeses")]
        [Required]
        public Int32 Successes { get; set; }

        public Int32 ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public String AgentId { get; set; }
        public virtual ApplicationUser Agent { get; set; }

        [Required]
        [Display(Name = "Campaign")]
        public Int32 CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        public Int32 TimeZoneId { get; set; }
        public virtual Entities.TimeZone TimeZone { get; set; }

        [Display(Name = "User")]
        public String UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

    }   
}


