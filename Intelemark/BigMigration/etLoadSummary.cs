namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etLoadSummary")]
    public partial class etLoadSummary
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string DataGroup { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Campaignid { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string Projectid { get; set; }

        [StringLength(10)]
        public string Source { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? Tot_Leads { get; set; }

        public int? Net_Leads { get; set; }

        public int? DupsByCompany_Load { get; set; }

        public int? DupsByCompany_DB { get; set; }

        public int? DupsByPhone_Load { get; set; }

        public int? DupsByPhone_DB { get; set; }

        public int? DupsByContact_Load { get; set; }

        public int? DupsByContact_DB { get; set; }

        [StringLength(100)]
        public string Load_Notes { get; set; }
    }
}
