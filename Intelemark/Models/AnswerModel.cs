using Intelemark.Entities;
using Intelemark.Models;
using Intelemark.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class AnswerModel : Model
    {
        [Display(Name = "Answer")]
        public String Answer { get; set; }

        [Display(Name = "Order")]
        public Int32 Order { get; set; }

        public Int32 QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}


