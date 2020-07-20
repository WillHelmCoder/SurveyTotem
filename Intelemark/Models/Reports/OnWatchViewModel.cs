using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models.Reports
{
    public class OnWatchViewModel : BaseReportModel
    {
        [Display(Name = "Campaign Name")]
        public String CampaignName { get; set; }

        [Display(Name = "Hours Limit")]
        public Double CampaignLimit { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Total Hours")]
        public Double? TotalHours { get; set; }

        public Double? HoursLeft { get; set; }
        public Boolean IsActive { get; set; }
    }
}