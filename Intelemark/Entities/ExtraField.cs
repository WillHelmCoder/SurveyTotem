using Intelemark.Models;
using Intelemark.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class ExtraField : Entity
    {
        public String FieldName { get; set; }
        public FieldTypes TypeId { get; set; }

        [ForeignKey("Campaign")]
        public Int32 CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; }

      //  public virtual List<ExtraFieldOption> ExtraFieldOptions { get; set; }
    }
}


