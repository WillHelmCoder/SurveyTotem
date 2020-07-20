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
    public class Answer : Entity
    {
        public String Name { get; set; }
        public Int32 Order { get; set; }

        [ForeignKey("Question")]
        public Int32 QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}


