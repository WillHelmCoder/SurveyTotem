using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class AnsweredFormModel : Model
    {
        public int QuestionId { get; set; }
        public virtual QuestionModel Question { get; set; }

        public String Answer { get; set; }
        public Boolean Checked { get; set; }

        public int RecordId { get; set; }
        public virtual RecordModel Record { get; set; }

        public List<CheckBoxAnswerModel> CheckBoxAnswer { get; set;}

        public String UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}