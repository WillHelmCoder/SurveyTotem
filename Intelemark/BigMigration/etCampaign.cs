namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etCampaign")]
    public partial class etCampaign
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public etCampaign()
        {
            etCallBacks = new HashSet<etCallBack>();
            etLookups = new HashSet<etLookup>();
            eUDFs = new HashSet<eUDF>();
        }

        [Key]
        [StringLength(10)]
        public string CampaignId { get; set; }

        [StringLength(50)]
        public string Campaign { get; set; }

        [StringLength(100)]
        public string CampaignDesc { get; set; }

        [StringLength(50)]
        public string CampaignObjective { get; set; }

        [StringLength(50)]
        public string Link1 { get; set; }

        [StringLength(15)]
        public string Link1Label { get; set; }

        [StringLength(50)]
        public string Link2 { get; set; }

        [StringLength(15)]
        public string Link2Label { get; set; }

        [StringLength(50)]
        public string Link3 { get; set; }

        [StringLength(15)]
        public string Link3Label { get; set; }

        [StringLength(10)]
        public string DailyHours { get; set; }

        [StringLength(10)]
        public string TimezoneUpdateFlag { get; set; }

        [Column(TypeName = "money")]
        public decimal? BillingPerHourProduction { get; set; }

        [Column(TypeName = "money")]
        public decimal? BillingPerHourTraining { get; set; }

        [Column(TypeName = "money")]
        public decimal? BillingPerSale { get; set; }

        [StringLength(30)]
        public string CampaignSupervisor { get; set; }

        [StringLength(10)]
        public string IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string MaxQueue { get; set; }

        [StringLength(50)]
        public string MaxAttempts { get; set; }

        [StringLength(1)]
        public string OnWatch { get; set; }

        public decimal? WatchHours { get; set; }

        [StringLength(15)]
        public string CPhone { get; set; }

        [StringLength(50)]
        public string Client_Number { get; set; }

        [StringLength(50)]
        public string QueueSortBy { get; set; }

        [StringLength(10)]
        public string CompanyLink { get; set; }

        [StringLength(10)]
        public string SpellNotes { get; set; }

        [Required]
        [StringLength(150)]
        public string DailyActivityReportPath { get; set; }

        [StringLength(1)]
        public string HoursTracking { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<etCallBack> etCallBacks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<etLookup> etLookups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eUDF> eUDFs { get; set; }
    }
}
