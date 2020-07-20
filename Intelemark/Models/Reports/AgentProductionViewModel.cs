using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models.Reports
{
    public class AgentProductionViewModel : BaseReportModel
    {
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Reported hours")]
        public double ReportedHours { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Reported training hours")]
        public double ReportedTrainingHours { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "SPH")]
        public double SuccessPerHour { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "3 day SPH")]
        public double ThreeDaySuccessPerHour { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "10 day SPH")]
        public double TenDaySuccessPerHour { get; set; }
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
        public double RevenuePerHour { get { return ((ReportedHours * (double)UserCampaign.Campaign.BillingPH) / (EndDate - StartDate).TotalHours); } }
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Total Revenue")]
        public double TotalRevenue { get { return ReportedHours * (double)UserCampaign.Campaign.BillingPH; } }


        public int AgentId { get; set; }



    }


    public class AgentProductionViewModelByCampaign : BaseReportModel
    {
        public List<AgentProductionViewModel> CampaignProduction;
    }


    public class PayRollPrepByDay : BaseReportModel
    {
        public DateTime ReportDay { get; set; }
        public IEnumerable<PayRollPrep> AgentReport { get; set; }
    }

    public class PayRollPrep : BaseReportModel
    {
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Reported hours")]
        public double ReportedHours { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Reported training hours")]
        public double ReportedTrainingHours { get; set; }
        [Display(Name = "Agent")]
        public virtual ApplicationUser Agent { get; set; }
        [Display(Name = "User Campaign")]
        public virtual UserCampaign UserCampaign { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public double DailyHours { get => UserCampaign.BudgetedHours / 7; }
    }

    public class PayRoll : BaseReportModel
    {
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Reported hours")]
        public double ReportedHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Reported hours")]
        public double HourPercentage { get => TotalBudgetedHours == 0 ? 0 : ((ReportedTrainingHours + ReportedHours) / TotalBudgetedHours) * 100; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Reported training hours")]
        public double ReportedTrainingHours { get; set; }

        [Display(Name = "Agent")]
        public virtual ApplicationUser Agent { get; set; }

        [Display(Name = "User Campaign")]
        public virtual UserCampaign UserCampaign { get; set; }

        [Display(Name = "Successes")]
        public int Successes { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Total Budget")]
        public double TotalBudgetedHours { get => (UserCampaign.BudgetedHours / 7) * (EndDate - StartDate).TotalDays; }

        [DisplayFormat(DataFormatString = "${0:0.##}")]
        public double DialingCompensation { get => ReportedHours * (UserCampaign.PayRateDialingHours - Penalty); }

        [DisplayFormat(DataFormatString = "${0:0.##}")]
        public double TrainingCompensation { get => ReportedTrainingHours *  (UserCampaign.PayRateTrainingHours - Penalty); }

        [DisplayFormat(DataFormatString = "${0:0.##}")]
        public double SuccessCompensation { get => Successes * UserCampaign.PayRateTrainingHours; }

        [DisplayFormat(DataFormatString = "${0:0.##}")]
        public double Penalty { get; set; }

        [DisplayFormat(DataFormatString = "${0:0.##}")]
        public double Total { get => DialingCompensation + TrainingCompensation + SuccessCompensation; } 
    }




}