using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public abstract class Model
    {
        [Key]
        [Display(Name = "Id")]
        public virtual int Id { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Last Update")]
        public DateTime LastUpdate { get; set; }

    }
}