using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class CampaignCallCode : Entity
    {
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        [ForeignKey("CallCode")]
        public int CallCodeId { get; set; }
        public virtual CallCode CallCode { get; set; }

    }
}