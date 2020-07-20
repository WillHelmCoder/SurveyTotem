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
    public class FormModel : Model
    {
        [Display(Name = "Name")]
        [Required]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public String Description { get; set; }

        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        [Required]
        public QuestionModel[] Questions { get; set; }

        public virtual List<QuestionModel> QuestionList { get; set; }

        [Required]
        [Display(Name = "Question Type")]
        public QuestionTypes QuestionType { get; set; }


    }
}


