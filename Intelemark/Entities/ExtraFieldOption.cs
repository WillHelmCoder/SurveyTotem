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
    public class ExtraFieldOption : Entity
    {
        public String FieldOptionName { get; set; }

        [ForeignKey("ExtraField")]
        public Int32 ExtraFieldId { get; set; }

        public virtual ExtraField ExtraField { get; set; }
    }
}


