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
    public class ExtraFieldModel : Model
    {
        [Required]
        [Display(Name = "Field Name")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "The field name must not contain special characters.")]
        public String FieldName { get; set; }

        [Required]
        [Display(Name = "Field Type")]
        public FieldTypes TypeId { get; set; }

        [ForeignKey("Campaign")]
        public Int32 CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; }

        public virtual List<ExtraFieldOptionModel> ExtraFieldOptions { get; set; }

    }
}


