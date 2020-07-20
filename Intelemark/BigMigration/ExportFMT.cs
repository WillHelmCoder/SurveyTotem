namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExportFMT")]
    public partial class ExportFMT
    {
        public int ExportFMTID { get; set; }

        [StringLength(50)]
        public string ExportName { get; set; }

        [StringLength(50)]
        public string CampaignID { get; set; }

        public int? FieldOrder { get; set; }

        [StringLength(50)]
        public string ExportField { get; set; }

        [StringLength(100)]
        public string LabelName { get; set; }
    }
}
