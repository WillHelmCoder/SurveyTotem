namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class etSession
    {
        [Key]
        public int SessionsId { get; set; }

        [Required]
        [StringLength(10)]
        public string CampaignID { get; set; }

        [StringLength(10)]
        public string AgentID { get; set; }

        public DateTime? Sdate { get; set; }

        public double? AgentReportedHours { get; set; }

        public double? AgentReportedTrainingHours { get; set; }

        public double? AgentReportedSales { get; set; }

        public double? AgentActualHours { get; set; }

        public double? AgentActualSales { get; set; }

        public int? AgentDials { get; set; }

        public double? AgentTalkTime { get; set; }

        [Column(TypeName = "money")]
        public decimal? PayHourly { get; set; }

        [Column(TypeName = "money")]
        public decimal? PaySales { get; set; }

        [Column(TypeName = "money")]
        public decimal? PayIncentives { get; set; }

        [Column(TypeName = "money")]
        public decimal? PayOther { get; set; }

        [StringLength(15)]
        public string PayNotes { get; set; }

        [Column(TypeName = "money")]
        public decimal? RevenueHourly { get; set; }

        [Column(TypeName = "money")]
        public decimal? RevenuePerSales { get; set; }

        [StringLength(10)]
        public string Active { get; set; }

        public decimal? CampaignHours { get; set; }
    }
}
