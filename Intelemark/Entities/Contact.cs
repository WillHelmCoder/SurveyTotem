using Intelemark.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intelemark.Entities
{
    public class Contact : Entity
    {
        //ctor...

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string AltPhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Notes { get; set; }

        public int? Extension { get; set; }
        public string Title { get; set; }
        public string SIC { get; set; }
        public string Company { get; set; }
        public string AltAddress { get; set; }
        public int Attempts { get; set; }
        public bool IsFinalized { get; set; }

        public bool InHold { get; set; }
        public bool OnDial { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LeadId { get; set; }

        public int TimeZoneId { get; set; }
        public virtual TimeZone TimeZone { get; set; }

        public string AgentId { get; set; }
        public virtual ApplicationUser Agent { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }



        [NotMapped]
        public string TimeZoneString { get; set; }

     //   public virtual List<ExtraFieldValue> ExtraFieldValues { get; set; }
    }
}