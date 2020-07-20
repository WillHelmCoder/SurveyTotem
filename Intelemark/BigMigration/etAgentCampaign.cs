namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etAgentCampaign")]
    public partial class etAgentCampaign
    {
        [Key]
        [Column(Order = 0)]
        public int etAgentCampaignid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string CampaignId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string AgentID { get; set; }

        public decimal? CampaignHours { get; set; }

        [StringLength(10)]
        public string DialMode { get; set; }

        [Column(TypeName = "money")]
        public decimal? PayPerHour { get; set; }

        [Column(TypeName = "money")]
        public decimal? PayPerSale { get; set; }

        [Column(TypeName = "money")]
        public decimal? PayPerTrnHour { get; set; }

        [StringLength(50)]
        public string IncentiveID1 { get; set; }

        [StringLength(50)]
        public string IncentiveID3 { get; set; }

        [StringLength(50)]
        public string IncentiveID2 { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(10)]
        public string IsActive { get; set; }
    }
}
