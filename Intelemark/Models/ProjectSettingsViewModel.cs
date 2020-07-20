using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class ProjectSettingsViewModel : Model
    {
        [Required]
        [Display(Name = "Project Name")]
        public String Name { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        public List<ProjectPriorityModel> ProjectPriorities { get; set; }

        public List<ProjectPriorityDetailModel> ProjectPriorityDetails { get; set; }

        public List<ContactModel> ProjectContacts { get; set; }
    }
}