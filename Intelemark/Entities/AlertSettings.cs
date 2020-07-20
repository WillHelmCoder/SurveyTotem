using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class AlertSettings : Entity
    {
        public String NotificationEmails { get; set; }
        public Double DataPercentage { get; set; }
    }
}


