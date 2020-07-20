namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("eUDF")]
    public partial class eUDF
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UDFId { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string CampaignId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string UDFNum { get; set; }

        [StringLength(35)]
        public string Label { get; set; }

        [StringLength(30)]
        public string Format { get; set; }

        [StringLength(25)]
        public string Mask { get; set; }

        [StringLength(20)]
        public string DefaultValue { get; set; }

        public int? Size { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(1)]
        public string SpellUDF { get; set; }

        public virtual etCampaign etCampaign { get; set; }
    }
}
