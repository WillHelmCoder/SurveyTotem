using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class ProjectManagementViewModel : Model
    {
        [Required]
        [Display(Name = "Project Name")]
        public String Name { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        [Display(Name = "ProjectId")]
        public Int32 ProjectId { get; set; }

        public List<ProjectManagementBooleanModel> ProjectTitles { get; set; }

        public List<ProjectManagementBooleanModel> ProjectCompanies { get; set; }

        public List<ProjectManagementBooleanModel> ProjectSICs { get; set; }

        public List<ProjectManagementBooleanModel> ProjectStates { get; set; }

        public List<ProjectManagementBooleanModel> ProjectCities { get; set; }

        public List<ProjectManagementBooleanModel> ProjectTimeZones { get; set; }

        public List<Int32> ProjectTimeZonesId { get; set; }


    }


    public class ProjectManagementBooleanModel : Model
    {
        public String Property { get; set; }
        public Boolean IsChecked { get; set; }
        public Entities.TimeZone TimeZone { get; set; }
    }
}