using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class UserProfileModel 
    {
        [Key]
        public String Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Full Name is Required")]
        public String Name { get; set; }

        [StringLength(100, ErrorMessage = "Number of characters {0} must be at least {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [Display(Name = "Contact")]
        public String Contact { get; set; }

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
        public String ZipCode { get; set; }

        [Display(Name = "Notes")]
        public String Notes { get; set; }

        [Display(Name = "Role")]
        public String Role { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public String PhoneNumber { get; set; }

        [Display(Name = "Available Hours")]
        public Double? AvailableHours { get; set; }
    }

}