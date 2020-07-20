using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class RecordModel : Model
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int ContactId { get; set; }
        public virtual ContactModel Contact { get; set; }
        public int FormId { get; set; }
        public virtual FormModel Form { get; set; }
        public int CallCodeId { get; set; }
        public virtual CallCodeModel CallCode { get; set; }
        public Int32 Seconds { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<AnsweredFormModel> FormAnswers { get; set; }

    }
}