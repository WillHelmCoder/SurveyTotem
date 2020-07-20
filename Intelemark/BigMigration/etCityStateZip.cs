namespace Intelemark.BigMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("etCityStateZip")]
    public partial class etCityStateZip
    {
        public int etCityStateZipId { get; set; }

        [StringLength(10)]
        public string Zip { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(10)]
        public string State { get; set; }

        [StringLength(10)]
        public string Timezone { get; set; }
    }
}
