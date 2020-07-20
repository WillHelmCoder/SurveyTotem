namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etCallHistory")]
    public partial class etCallHistory
    {
        [Key]
        [Column(Order = 0)]
        public int CallHistoryId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string CampaignId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LeadID { get; set; }

        [StringLength(10)]
        public string AgentID { get; set; }

        [StringLength(20)]
        public string ProjectId { get; set; }

        [StringLength(10)]
        public string EOC { get; set; }

        public DateTime? CallDate { get; set; }

        [StringLength(10)]
        public string CallType { get; set; }

        public int? CallDuration { get; set; }

        [StringLength(1)]
        public string DecisionMaker { get; set; }

        [StringLength(1)]
        public string Presentation { get; set; }
    }
}
