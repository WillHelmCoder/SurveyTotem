using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models.Reports
{
    public class AgentCallAnalysisViewModel : BaseReportModel
    {
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Reported hours")]
        public double ReportedHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Reported training hours")]
        public double ReportedTrainingHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Success / hour")]
        public double SuccessPerHour { get; set; }

        [Display(Name = "Successes")]
        public int Successes { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Dials per hour")]
        public double DialsPerHour { get; set; }

        [Display(Name = "Agent")]
        public virtual ApplicationUser Agent { get; set; }

        [Display(Name = "User Campaign")]
        public virtual UserCampaign UserCampaign { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Compensation / hour")]
        public double CompensationPerHour { get { return ((ReportedTrainingHours * UserCampaign.PayRateTrainingHours) + (ReportedHours * UserCampaign.PayRateDialingHours) + (Successes * UserCampaign.PayRateSuccess) / (EndDate - StartDate).TotalHours);  } }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Total Compensation")]
        public double TotalCompensation { get { return (ReportedTrainingHours * UserCampaign.PayRateTrainingHours) + (ReportedHours * UserCampaign.PayRateDialingHours) + (Successes * UserCampaign.PayRateSuccess); } }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Revenue / hour")]
        public double RevenuePerHour { get { return ((ReportedTrainingHours * UserCampaign.PayRateTrainingHours) + (ReportedHours * UserCampaign.PayRateDialingHours) + (Successes * UserCampaign.PayRateSuccess) / (EndDate - StartDate).TotalHours); } }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Total Revenue")]
        public double TotalRevenue { get { return (ReportedTrainingHours * UserCampaign.PayRateTrainingHours) + (ReportedHours * UserCampaign.PayRateDialingHours) + (Successes * UserCampaign.PayRateSuccess); } }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Actual Hours")]
        public Double? ActualHours { get; set; }

        [Display(Name = "Total Dials")]
        public Int32? TotalDials { get; set; }

        [Display(Name = "Hours Diff.")]
        public string HoursDiff { get => Math.Abs(ReportedHours - (double)ActualHours).ToString("F2"); }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Minimum Call Time")]
        public Double? MinimumCallTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Maximum Call Time")]
        public Double? MaximumCallTime { get; set; }



    }
}