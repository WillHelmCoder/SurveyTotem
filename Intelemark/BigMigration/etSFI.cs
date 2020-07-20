namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etSFI")]
    public partial class etSFI
    {
        public int etSFIID { get; set; }

        [Required]
        [StringLength(10)]
        public string CampaignId { get; set; }

        [Required]
        [StringLength(70)]
        public string SFUserID { get; set; }

        [Required]
        [StringLength(25)]
        public string SFPassword { get; set; }

        [Required]
        [StringLength(70)]
        public string SFToken { get; set; }

        [Required]
        [StringLength(70)]
        public string SFLeadSource { get; set; }
    }
}
