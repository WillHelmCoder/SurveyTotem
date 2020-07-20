using Intelemark.Models;
using Intelemark.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class Appointments : Entity
    {
        public String Notes { get; set; }

        public DateTime DateScheduled { get; set; }
        public DateTime ReminderDate { get; set; }

        public Int32 CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        public Int32? RecordId { get; set; }
        public virtual Record Record { get; set; }

        public String AgentId { get; set; }
        public virtual ApplicationUser Agent { get; set; }

        public Boolean IsConfirmed { get; set; }
        public Boolean Notified { get; set; }
        public Int32 TimesScheduled { get; set; }

        //TRUE: Callback - FALSE: Cita
        public Boolean IsScheduled { get; set; }
    }
}


