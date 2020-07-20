using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models.Reports
{
    public class BaseReportAgentModel : Model
    {
        [Display(Name = "Agent")]
        public String AccountManagerId { get; set; }
        public virtual ApplicationUser AccountManager { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}