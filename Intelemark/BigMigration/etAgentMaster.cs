namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etAgentMaster")]
    public partial class etAgentMaster
    {
        [Key]
        [StringLength(10)]
        public string AgentID { get; set; }

        [StringLength(50)]
        public string AgentName { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        [StringLength(15)]
        public string State { get; set; }

        [StringLength(12)]
        public string Zip { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(15)]
        public string Cell { get; set; }

        public DateTime? AgentStartDate { get; set; }

        public DateTime? AgentStopDate { get; set; }

        public DateTime? AgentBirthDate { get; set; }

        [StringLength(10)]
        public string PayMethod { get; set; }

        [StringLength(10)]
        public string SecurityLevel { get; set; }

        [StringLength(50)]
        public string AgentPassword { get; set; }

        public decimal? TotalHours { get; set; }

        [StringLength(10)]
        public string IsActive { get; set; }

        [StringLength(5)]
        public string AgentLevel { get; set; }

        [StringLength(100)]
        public string Experience { get; set; }

        [StringLength(100)]
        public string Notes { get; set; }
    }
}
