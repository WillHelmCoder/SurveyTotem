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
    public class Project : Entity
    {
        //public Project()
        //{
        //    Records = new List<Record>();
        //    Contacts = new List<Contact>();
        //}

        public string Name { get; set; }
        public string Description { get; set; }
        public Priorities Priority { get; set; }
        public int CampaignId { get; set; }

        [NotMapped]
        public string ConcatCampaignProject { get => $"{Campaign.Identifier} - {Name}"; }

        public virtual Campaign Campaign { get; set; }


        public virtual List<ProjectPriority> ProjectPriority { get; set; }
        public List<Contact> Contacts { set; get; }

        //public List<Record> Records { get; set; }
        //public List<Contact> Contacts { get; set; }
        //FEX: missing entity
        //public List<ApplicationUser> Agents { get; set; }
    }
}