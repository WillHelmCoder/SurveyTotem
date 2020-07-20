using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models.Reports
{
    public class CallAnalysisByTimeOfDayViewModel: BaseReportModel
    {
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Reported hours")]
        public double ReportedHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Success / hour")]
        public double SuccessPerHour { get; set; }

        [Display(Name = "Successes")]
        public int Successes { get; set; }

        [Display(Name = "Successes")]
        public int Completes { get; set; }

        [Display(Name = "Agent")]
        public virtual ApplicationUser Agent { get; set; }

        [Display(Name = "User Campaign")]
        public virtual UserCampaign UserCampaign { get; set; }

        [Display(Name = "Total Dials")]
        public Int32? TotalDials { get; set; }

    }


    public class CallAnalysisByTimeOfDayGroup : BaseReportModel
    {
        public DateTime Hours { get; set; }
        public List<CallAnalysisByTimeOfDayViewModel> CallAnalyisisGroupedList { get; set; }
    }
}