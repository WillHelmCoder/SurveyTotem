using Intelemark.Entities;
using Intelemark.Models;
using Intelemark.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class ExtraFieldOptionModel : Model
    {
        [Display(Name = "Field Option")]
        public String FieldOptionName { get; set; }

        [ForeignKey("ExtraField")]
        public Int32 ExtraFieldId { get; set; }

        public virtual ExtraFieldModel ExtraField { get; set; }
    }
}


