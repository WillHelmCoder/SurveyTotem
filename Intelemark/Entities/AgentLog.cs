using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class AgentLog : Entity
    {
        public Double DialingHours { get; set; }
        public Double TrainingHours { get; set; }
        public Int32 Successes { get; set; }

        public String AgentId { get; set; }
        public virtual ApplicationUser Agent { get; set; }

        public Int32 ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public Int32 CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        public Int32 TimeZoneId { get; set; }
        public virtual TimeZone TimeZone { get; set; }
    }
}


