using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class Form : Entity
    {
        public String Name { get; set; }
        public String Description { get; set; }

        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual List<Question> Questions { get; set; } = new List<Question>();
    }
}


