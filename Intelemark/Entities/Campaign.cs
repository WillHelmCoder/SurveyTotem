using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class Campaign : Entity
    {
        public Campaign()
        {
            Projects = new List<Project>();
            //Appointments = new List<Appointments>();
        }

        [StringLength(10)]
        public String Identifier { get; set; }
        public String Description { get; set; }
        public String Objective { get; set; }
        public Decimal BillingPH { get; set; }
        public Boolean CompanyLink { get; set; }
        public Boolean ActiveControl { get; set; }
        public Boolean SpellCheck { get; set; }
        public int MaxAttempt { get; set; }
        public Double CampaignLimit { get; set; }

        public String AccountManagerId { get; set; }
        public virtual ApplicationUser AccountManager { get; set; }

        public String ClientId { get; set; }
        public virtual ApplicationUser Client { get; set; }

        public virtual List<ExtraField> ExtraFields { get; set; }
        public virtual ICollection<Form> Forms { get; set; }

        public List<Project> Projects { set; get; }
        //public List<Appointments> Appointments { set; get; }
    }
}


