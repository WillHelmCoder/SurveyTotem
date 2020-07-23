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
    public class Question : Entity
    {
        public String Name { get; set; }
        public QuestionTypes TypeId { get; set; }
        public Int32 Order { get; set; }
        public Int32 Points { get; set; }
        public String Image { get; set; }

        [ForeignKey("Form")]
        public Int32 FormId { get; set; }

        public virtual Form Form { get; set; }

        public virtual List<Answer> Answers { get; set; }
    }
}


