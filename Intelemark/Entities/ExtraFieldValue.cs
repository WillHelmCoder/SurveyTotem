using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class ExtraFieldValue : Entity
    {
        [ForeignKey("ExtraField")]
        public int ExtraFieldId { get; set; }
        public virtual ExtraField ExtraField { get; set; }

        public string Value { get; set; }
        public Int32 ContactId { get; set; }
        public Contact Contact { get; set; }

    }
}