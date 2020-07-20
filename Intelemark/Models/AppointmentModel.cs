using Intelemark.Entities;
using Intelemark.Models;
using Intelemark.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class AppointmentModel : Model
    {
        [Required]
        [Display(Name = "Name/Notes")]
        public String Notes { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime DateScheduled { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Campaign")]
        public Int32 CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        [ScaffoldColumn(false)]
        public Int32? RecordId { get; set; }
        public virtual Record Record { get; set; }

        [Display(Name = "Agent/User")]
        public String AgentId { get; set; }
        public virtual ApplicationUser Agent { get; set; }

        [Display(Name = "Is Confirmed?")]
        public Boolean IsConfirmed { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Is Scheduled?")]
        public Boolean IsScheduled { get; set; }

        [Display(Name = "Times Rescheduled")]
        public Int32 TimesScheduled { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

    }
}


