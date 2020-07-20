namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etLoadMore")]
    public partial class etLoadMore
    {
        public int etLoadMoreID { get; set; }

        [Required]
        [StringLength(10)]
        public string CampaignId { get; set; }

        public int? LeadID { get; set; }

        [StringLength(500)]
        public string UDF17 { get; set; }

        [StringLength(500)]
        public string UDF18 { get; set; }

        [StringLength(500)]
        public string UDF19 { get; set; }

        [StringLength(500)]
        public string UDF20 { get; set; }

        [StringLength(500)]
        public string UDF21 { get; set; }

        [StringLength(500)]
        public string UDF22 { get; set; }

        [StringLength(500)]
        public string UDF23 { get; set; }

        [StringLength(500)]
        public string UDF24 { get; set; }

        [StringLength(500)]
        public string UDF25 { get; set; }

        [StringLength(500)]
        public string UDF26 { get; set; }

        [StringLength(500)]
        public string UDF27 { get; set; }

        [StringLength(500)]
        public string UDF28 { get; set; }

        [StringLength(500)]
        public string UDF29 { get; set; }

        [StringLength(500)]
        public string UDF30 { get; set; }

        [StringLength(500)]
        public string UDF31 { get; set; }

        [StringLength(500)]
        public string UDF32 { get; set; }

        [StringLength(10)]
        public string ProjectId { get; set; }

        [StringLength(10)]
        public string DataGroup { get; set; }

        [StringLength(10)]
        public string Source { get; set; }
    }
}
