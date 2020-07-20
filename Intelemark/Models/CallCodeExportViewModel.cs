using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class CallCodeExportViewModel : Model
    {
        [Required]
        [Display(Name = "Exporting Campaign")]
        public int ExportId { get; set; }

        [Required]
        [Display(Name = "Importing Campaign")]
        public int InNOutMonsterFries { get; set; }

        
    }
}