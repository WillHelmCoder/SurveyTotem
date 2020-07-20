using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class SingleFormModel : Model
    {
      
        public FormModel Form { get; set; }
        public RecordModel Record { get; set; }

        public List<AnsweredForm> FormAnswers { get; set; }
        public List<AnsweredFormModel> FormAnswersModel { get; set; }
    }
}