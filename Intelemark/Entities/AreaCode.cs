using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class AreaCode : Entity
    {
        public String Name { get; set; }
        public String TZ { get; set; }

        public Int32 TimeZoneId { get; set; }
        public virtual TimeZone TimeZone { get; set; }
    }
}


