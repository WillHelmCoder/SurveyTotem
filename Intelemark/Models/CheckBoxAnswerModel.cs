using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class CheckBoxAnswerModel : Model
    {
        public bool Checked { get; set; }
        public string Answer { get; set; }
    }
}