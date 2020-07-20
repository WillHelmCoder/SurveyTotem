namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("elMaster")]
    public partial class elMaster
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string CampaignId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LeadID { get; set; }

        [StringLength(50)]
        public string Contact { get; set; }

        [StringLength(50)]
        public string Company { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Address1 { get; set; }

        [StringLength(50)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(10)]
        public string State { get; set; }

        [StringLength(20)]
        public string Zip { get; set; }

        [StringLength(15)]
        public string Phone1 { get; set; }

        [StringLength(10)]
        public string PhoneExt { get; set; }

        [StringLength(15)]
        public string DirLine { get; set; }

        [StringLength(15)]
        public string Mobile { get; set; }

        [StringLength(15)]
        public string FaxNumber { get; set; }

        [StringLength(65)]
        public string Email { get; set; }

        [StringLength(75)]
        public string WebURL { get; set; }

        [StringLength(50)]
        public string BusType { get; set; }

        public int? NumEmpl { get; set; }

        [StringLength(15)]
        public string SalesVol { get; set; }

        [StringLength(18)]
        public string SIC { get; set; }

        [StringLength(10)]
        public string TimeZone { get; set; }

        [StringLength(10)]
        public string ProjectId { get; set; }

        [StringLength(50)]
        public string AgentId { get; set; }

        [StringLength(10)]
        public string EOC { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public int? Attempts { get; set; }

        public DateTime? LCDate { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [StringLength(500)]
        public string UDF1 { get; set; }

        [StringLength(500)]
        public string UDF2 { get; set; }

        [StringLength(500)]
        public string UDF3 { get; set; }

        [StringLength(500)]
        public string UDF4 { get; set; }

        [StringLength(500)]
        public string UDF5 { get; set; }

        [StringLength(500)]
        public string UDF6 { get; set; }

        [StringLength(500)]
        public string UDF7 { get; set; }

        [StringLength(500)]
        public string UDF8 { get; set; }

        [StringLength(500)]
        public string UDF9 { get; set; }

        [StringLength(500)]
        public string UDF10 { get; set; }

        [StringLength(500)]
        public string UDF11 { get; set; }

        [StringLength(500)]
        public string UDF12 { get; set; }

        [StringLength(500)]
        public string UDF13 { get; set; }

        [StringLength(500)]
        public string UDF14 { get; set; }

        [StringLength(500)]
        public string UDF15 { get; set; }

        [StringLength(500)]
        public string UDF16 { get; set; }

        [StringLength(15)]
        public string DataGroup { get; set; }

        [StringLength(10)]
        public string Source { get; set; }

        [StringLength(10)]
        public string IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(10)]
        public string RecordLock { get; set; }

        [StringLength(15)]
        public string Country { get; set; }
    }
}
