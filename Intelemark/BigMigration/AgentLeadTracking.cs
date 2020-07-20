namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AgentLeadTracking")]
    public partial class AgentLeadTracking
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LeadId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string CampaignId { get; set; }

        [Required]
        [StringLength(20)]
        public string AgentId { get; set; }

        public DateTime ADate { get; set; }
    }
}
