namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class etAgentIncentive
    {
        [Key]
        [StringLength(10)]
        public string IncentiveID { get; set; }

        [StringLength(10)]
        public string EOC { get; set; }

        [StringLength(20)]
        public string IncentiveType { get; set; }

        [StringLength(20)]
        public string PayMethod { get; set; }

        public decimal? MeasureLevel1 { get; set; }

        public decimal? MeasureLevel2 { get; set; }

        public decimal? MeasureLevel3 { get; set; }

        public decimal? MeasureLevel4 { get; set; }

        public decimal? MeasureLevel5 { get; set; }

        public decimal? MeasureLevel6 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PayLevel1 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PayLevel2 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PayLevel3 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PayLevel4 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PayLevel5 { get; set; }

        [Column(TypeName = "money")]
        public decimal? PayLevel6 { get; set; }
    }
}
