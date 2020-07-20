namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etEOC")]
    public partial class etEOC
    {
        [Key]
        public int EOCid { get; set; }

        [StringLength(10)]
        public string CampaignId { get; set; }

        [StringLength(10)]
        public string EOC { get; set; }

        [StringLength(50)]
        public string EOCDesc { get; set; }

        [StringLength(10)]
        public string Recyclable { get; set; }

        [StringLength(10)]
        public string Actions { get; set; }

        [StringLength(10)]
        public string Display { get; set; }

        [StringLength(10)]
        public string IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(10)]
        public string Type { get; set; }

        [StringLength(10)]
        public string InSPH { get; set; }

        [StringLength(1)]
        public string DMPrompt { get; set; }
    }
}
