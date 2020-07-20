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
    public class PenaltyModel : Model
    {
        [Display(Name = "Pay Rate")]
        [Required]
        public Double PayRate { get; set; }

        [Display(Name = "From")]
        [Required]
        public Double From { get; set; }

        [Display(Name = "To")]
        [Required]
        public Double To { get; set; }

        [Display(Name = "Penalty")]
        [Required]
        public Double PenaltyFee { get; set; }
    }
}


