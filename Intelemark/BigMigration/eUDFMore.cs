namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("eUDFMore")]
    public partial class eUDFMore
    {
        [Key]
        [Column(Order = 0)]
        public int UDFId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string CampaignId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LeadId { get; set; }

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
    }
}
