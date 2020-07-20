using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Intelemark.Entities;
using Intelemark.Utilities;

namespace Intelemark.Models
{
    public class UserProjectModel : Model
    {
        [Required]
        [Display(Name = "User")]
        public String UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Display(Name = "Description")]
        public Int32 ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}