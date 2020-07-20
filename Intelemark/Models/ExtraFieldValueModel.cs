using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class ExtraFieldValueModel : Model
    {
        public int ExtraFieldId { get; set; }
        public virtual ExtraFieldModel ExtraField { get; set; }
        public Boolean Checked { get; set; }
        public string Value { get; set; }

        public Int32 ContactId { get; set; }
        public Contact Contact { get; set; }

    }
}