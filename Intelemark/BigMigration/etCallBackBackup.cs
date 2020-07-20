namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etCallBackBackup")]
    public partial class etCallBackBackup
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CallbackId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string CampaignId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LeadID { get; set; }

        [StringLength(10)]
        public string AgentID { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [StringLength(50)]
        public string Contact { get; set; }

        [StringLength(15)]
        public string Phone1 { get; set; }

        [StringLength(50)]
        public string CallbackDate01 { get; set; }

        [StringLength(50)]
        public string CallbackTime01 { get; set; }

        [StringLength(500)]
        public string Comments { get; set; }

        [StringLength(1)]
        public string Reminded { get; set; }

        public DateTime? CBDateTime { get; set; }

        public DateTime? SetupDateTime { get; set; }
    }
}
