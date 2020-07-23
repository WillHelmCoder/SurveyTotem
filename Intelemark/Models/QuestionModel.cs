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
    public class QuestionModel : Model
    {
        [Required]
        [Display(Name = "Question")]
        public String Question { get; set; }

        [Required]
        [Display(Name = "Question Type")]
        public QuestionTypes TypeId { get; set; }

        [Display(Name = "Order")]
        public Int32 Order { get; set; }
        public Int32 Points { get; set; }
        public String Image { get; set; }
        public Int32 FormId { get; set; }
        public Form Form { get; set; }

        public AnswerModel[] Answers { get; set; }

        public virtual List<AnswerModel> AnswerList { get; set; }



    }
}


