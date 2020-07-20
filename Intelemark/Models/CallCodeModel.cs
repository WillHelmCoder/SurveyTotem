using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Intelemark.Entities;
using Intelemark.Utilities;

namespace Intelemark.Models
{
    public class CallCodeModel : Model
    {
        [Required]
        [Display(Name = "Description")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "End of Call Code")]
        public String Code { get; set; }

        public string DropDownDisplay { get => $"{Code} - {Name}"; }

        [Required]
        [Display(Name = "Behavior")]
        public EOCBehaviors Behavior { get; set; }

        [Required]
        [Display(Name = "Count as success")]
        public Boolean IsSuccess { get; set; }

        public Int32 CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        public Int32? Count { get; set; }
        
    }
}