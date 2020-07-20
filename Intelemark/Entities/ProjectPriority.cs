using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class ProjectPriority : Entity
    {
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public string Field { get; set; }
        public int PriorityValue { get; set; }
        public virtual List<ProjectPriorityDetail> ProjectPriorityDetails { get; set; }
    }
}