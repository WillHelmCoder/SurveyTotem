using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class StateRestrictionModel : Model
    {
        [Display(Name = "Abbreviation")]
        public String Abbreviation { get; set; }

        [Display(Name = "Name")]
        public String Name { get; set; }

        [Display(Name = "Is Restricted?")]
        public Boolean IsRestricted { get; set; }
    }
}