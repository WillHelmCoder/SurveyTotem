using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class ProjectPriorityModel : Model
    {
        public int ProjectId { get; set; }
        public virtual ProjectModel Project { get; set; }
        public string Field { get; set; }
        public int PriorityValue { get; set; }
        public virtual List<ProjectPriorityDetailModel> ProjectPriorityDetails { get; set; }
    }
}