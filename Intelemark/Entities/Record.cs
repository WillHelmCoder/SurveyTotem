using Intelemark.Models;
using System;
using System.Collections.Generic;

namespace Intelemark.Entities
{
    public class Record : Entity
    {
        public int Duration { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        public int? FormId { get; set; }
        public virtual Form Form { get; set; }

        //public int? ProjectId { set; get; }
        //public Project Project { set; get; }

        public int CallCodeId { get; set; }
        public virtual CallCode CallCode { get; set; }

        public virtual List<AnsweredForm> AnsweredForms { get; set; }
    }
}