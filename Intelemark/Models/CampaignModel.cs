using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class CampaignModel : Model
    {

        [StringLength(10)]
        [Required]
        [Display(Name = "Identifier")]
        public String Identifier { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        [Display(Name = "Objective")]
        public String Objective { get; set; }

        [Display(Name = "Billing Per Hour")]
        public Decimal BillingPH { get; set; }

        [Display(Name = "Max Attempt")]
        public int MaxAttempt { get; set; }

        [Display(Name = "Company Link")]
        public Boolean CompanyLink { get; set; }

        [Display(Name = "Active Control")]
        public Boolean ActiveControl { get; set; }

        [Display(Name = "Spell Check")]
        public Boolean SpellCheck { get; set; }

        [Display(Name = "Campaign Limit Hours")]
        public Double CampaignLimit { get; set; }

        public String AccountManagerId { get; set; }
        public virtual ApplicationUser AccountManager { get; set; }

        public String ClientId { get; set; }
        public virtual ApplicationUser Client { get; set; }

        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        public virtual List<ExtraFieldModel> ExtraFields { get; set; }

        public List<StoredFileModel> StoredFiles { get; set; }

    }
}


