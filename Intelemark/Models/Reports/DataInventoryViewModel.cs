using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models.Reports
{
    public class DataInventoryViewModel : BaseReportModel
    {
        public String Status { get; set; }

        [Display(Name = "Campaign Name")]
        public String CampaignName { get; set; }

        [Display(Name = "Project Name")]
        public String ProjectName { get; set; }

        [Display(Name = "Time Zone")]
        public String TimeZoneName { get; set; }
        public String Contact { get; set; }

        [Display(Name = "1")]
        public Int32 Attempt1 { get; set; }

        [Display(Name = "2")]
        public Int32 Attempt2 { get; set; }

        [Display(Name = "3")]
        public Int32 Attempt3 { get; set; }

        [Display(Name = "4")]
        public Int32 Attempt4 { get; set; }

        [Display(Name = "5")]
        public Int32 Attempt5 { get; set; }

        [Display(Name = "6")]
        public Int32 Attempt6 { get; set; }

        [Display(Name = "Attempted Times")]
        public Int32 AttemptLevel { get; set; }

        public List<string> TimeZones { get; set; }

    }

    public class DataInventoryRViewModel : BaseReportModel
    {
        public String Status { get; set; }

        [Display(Name = "Campaign Name")]
        public String CampaignName { get; set; }

        [Display(Name = "Project Name")]
        public String ProjectName { get; set; }

        [Display(Name = "Agent")]
        public String Agent { get; set; }

        [Display(Name = "Time Zone")]
        public String TimeZoneName { get; set; }
        public String Contact { get; set; }


        [Display(Name = "0")]
        public Int32 Attempt0 { get; set; }

        [Display(Name = "1")]
        public Int32 Attempt1 { get; set; }

        [Display(Name = "2")]
        public Int32 Attempt2 { get; set; }

        [Display(Name = "3")]
        public Int32 Attempt3 { get; set; }

        [Display(Name = "4")]
        public Int32 Attempt4 { get; set; }

        [Display(Name = "5")]
        public Int32 Attempt5 { get; set; }

        [Display(Name = "6")]
        public Int32 Attempt6 { get; set; }

        [Display(Name = ">6")]
        public Int32 MoreThan6 { get; set; }

        [Display(Name = "Final")]
        public Int32 Final { get; set; }

        [Display(Name = "Hold")]
        public Int32 Hold { get; set; }

        [Display(Name = "Callbacks")]
        public Int32 Callbacks { get; set; }

        [Display(Name = "Total")]
        public Int32 Total { get; set; }

        [Display(Name = "Penetration")]
        public string Penetration { get; set; }

        [Display(Name = "Attempted Times")]
        public Int32 AttemptLevel { get; set; }
        public List<string> TimeZones { get; set; }

    }

    public class DataInventoryByCampaign : BaseReportModel
    {
        public List<DataInventoryRViewModel> CampaignInventory { get; set; }
        public string ProjectName { get; set; }
    }

    public class DataInventoryByTimeZone : BaseReportModel
    {
        public List<DataInventoryRViewModel> CampaignInventory { get; set; }
        public string TimeZone { get; set; }
    }

}