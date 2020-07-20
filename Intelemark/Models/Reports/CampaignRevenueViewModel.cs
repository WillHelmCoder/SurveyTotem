using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models.Reports
{
    public class CampaignRevenueViewModel : BaseReportModel
    {
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Campaign hours")]
        public double CampaignHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Training hours")]
        public double TrainingHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Budgeted hours")]
        public double BudgetedHours { get; set; }

        [Display(Name = "Total Successes")]
        public int Successes { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Successes / Hour")]
        public double SuccessesPerHour { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Revenue / Hour")]
        public double RevenuePerHour { get => CampaignHours * Double.Parse(Campaign.BillingPH.ToString()); }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Labor Cost")]
        public double LaborCost { get => AgentProductionList.Select(x => x.TotalCompensation).DefaultIfEmpty(0).Sum(); }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Net Revenue")]
        public double NetRevenue { get => (RevenuePerHour * (EndDate - StartDate).TotalHours) - LaborCost; }

        public List<AgentProductionViewModel> AgentProductionList { get; set; }

    }

    public class CampaignRevenueViewModelByDay : BaseReportModel
    {
        public DateTime ReportDay { get; set; }
        public IEnumerable<CampaignRevenueViewModel> CampaignRevenueReport { get; set; }
    }
}