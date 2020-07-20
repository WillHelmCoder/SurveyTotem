using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class SelectableAnswerViewModel
    {
        public Int32 CampaignId { get; set; }
        public Int32 FormId { get; set; }
        public Int32 QuestionId { get; set; }

        [Display(Name = "New Answer")]
        public String NewAnswer { get; set; }
    }
}