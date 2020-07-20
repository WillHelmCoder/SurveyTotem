namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etLookup")]
    public partial class etLookup
    {
        [Key]
        public int LookupId { get; set; }

        [Required]
        [StringLength(10)]
        public string CampaignId { get; set; }

        [StringLength(50)]
        public string Tablename { get; set; }

        [StringLength(50)]
        public string TValue { get; set; }

        public virtual etCampaign etCampaign { get; set; }
    }
}
