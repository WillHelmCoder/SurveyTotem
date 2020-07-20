using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class ProjectPriorityDetail : Entity
    {
        public int ProjectPriorityId { get; set; }
        public virtual ProjectPriority ProjectPriority { get; set; }
        public string FieldValue { get; set; }
        public int FieldPriorityValue { get; set; }
    }
}