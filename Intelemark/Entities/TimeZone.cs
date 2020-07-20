using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class TimeZone : Entity
    {
        public String Name { get; set; }
        public String Code { get; set; }
        public Int32 STD { get; set; }
        public Int32 DST { get; set; }
        public Int32 CurrentTime { get; set; }
    }
}


