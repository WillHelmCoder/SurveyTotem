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
    public class StateRestriction : Entity
    {
        public String Abbreviation { get; set; }
        public String Name { get; set; }
        public Boolean IsRestricted { get; set; }

        public Int32 TimeZoneId { get; set; }
        public virtual TimeZone TimeZone { get; set; }
    }
}


