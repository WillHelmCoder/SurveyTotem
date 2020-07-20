using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class OnWatchSettings : Entity
    {
        public virtual Campaign Campaign { get; set; }
        public Int32 CampaignId { get; set; }
        public Double HoursLeft { get; set; }
    }
}


