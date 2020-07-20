namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etMessage")]
    public partial class etMessage
    {
        [Key]
        public int MessageId { get; set; }

        public DateTime? Mdate { get; set; }

        [StringLength(20)]
        public string CampaignId { get; set; }

        [StringLength(20)]
        public string AgentId { get; set; }

        [StringLength(100)]
        public string Heading { get; set; }

        [StringLength(500)]
        public string Note { get; set; }
    }
}
