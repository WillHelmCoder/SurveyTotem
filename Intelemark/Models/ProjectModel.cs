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
    public class ProjectModel : Model
    {
        [Required]
        [Display(Name = "Project Name")]
        public String Name { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        [Display(Name = "Priority")]
        public Priorities Priority { get; set; }

        public Int32 CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; }


        public String DisplayName { get => $"{Name} - {Description}"; } 

    }
}


