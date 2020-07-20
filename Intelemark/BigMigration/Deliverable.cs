namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Deliverable
    {
        [Key]
        [Column(Order = 0)]
        public int DeliverableID { get; set; }

        [StringLength(50)]
        public string Campaignid { get; set; }

        [StringLength(50)]
        public string EOC { get; set; }

        [StringLength(50)]
        public string Layout { get; set; }

        [StringLength(50)]
        public string ExportFormat { get; set; }

        [StringLength(200)]
        public string TransDirectory { get; set; }

        [StringLength(50)]
        public string FileName { get; set; }

        [StringLength(50)]
        public string FieldsChanged { get; set; }

        [StringLength(50)]
        public string Char { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string QC { get; set; }
    }
}
