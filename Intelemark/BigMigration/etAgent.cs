namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etAgent")]
    public partial class etAgent
    {
        public int etAgentID { get; set; }

        [Required]
        [StringLength(10)]
        public string AgentID { get; set; }

        [Required]
        [StringLength(10)]
        public string CampaignID { get; set; }

        [StringLength(18)]
        public string ProjectId { get; set; }

        [StringLength(18)]
        public string TimeZone { get; set; }

        [StringLength(10)]
        public string IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string Client_Timezone { get; set; }
    }
}
