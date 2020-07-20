using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class AlertSettingsModel : Model
    {
        [Display(Name = "Notification Emails")]
        public String NotificationEmails { get; set; }

        [Display(Name = "Data Percentage")]
        public Double DataPercentage { get; set; }
    }
}