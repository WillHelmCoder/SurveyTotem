using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models.Reports
{
    public class AgentAvailabilityViewModel : BaseReportModel
    {

        [Display(Name = "Agent")]
        public virtual ApplicationUser Agent { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Available Hours")]
        public Double? AvailableHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Budgeted Hours")]
        public Double? BudgetedHours { get; set; }

        [Display(Name = "Status")]
        public String Status { get; set; }
    }
}