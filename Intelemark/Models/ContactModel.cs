using Intelemark.Entities;
using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class ContactModel : Model
    {
        [StringLength(50)]
        [Required]
        [Display(Name = "Contact Name")]
        public String Name { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        public String PhoneNumber { get; set; }

        [Display(Name = "Alternative Phone Number")]
        public String AltPhoneNumber { get; set; }

        [Display(Name = "Email")]
        public String Email { get; set; }

        [Display(Name = "Address")]
        public String Address { get; set; }

        [Display(Name = "City")]
        public String City { get; set; }

        [Display(Name = "State")]
        public String State { get; set; }

        [Display(Name = "Country")]
        public String Country { get; set; }

        [Display(Name = "Zip Code")]
        [StringLength(5)]
        public String ZipCode { get; set; }

        [Display(Name = "Error Code")]
        public String Error { get; set; }

        public Int32 ProjectId { get; set; }
        public ProjectModel Project { get; set; }

        public Int32 TimeZoneId { get; set; }
        public Intelemark.Entities.TimeZone TimeZone { get; set; }

        [Display(Name = "Extension")]
        public Int32? Extension { get; set; }

        [Display(Name = "Title")]
        public String Title { get; set; }

        [Display(Name = "Company")]
        public String Company { get; set; }

        [Display(Name = "SIC")]
        public String SIC { get; set; }

        [Display(Name = "Notes")]
        public String Notes { get; set; }

        [Display(Name = "Is Finalized?")]
        public Boolean IsFinalized { get; set; }

        [Display(Name = "Address Line 2")]
        public String AltAddress { get; set; }

        public Int32 Attempts { get; set; }

        public Boolean InHold { get; set; }

        public String AgentId { get; set; }

        public virtual AgentModel Agent { get; set; }

        public List<ExtraFieldModel> ExtraFields { get; set; }

        public List<ExtraFieldValueModel> ExtraFieldValues { get; set; }

        public List<AnsweredForm> FormAnswers { get; set; }



    }
}


