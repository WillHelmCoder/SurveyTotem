using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Intelemark.Entities
{
    public class Penalty : Entity
    {
        public Double PayRate { get; set; }
        public Double From { get; set; }
        public Double To { get; set; }
        public Double PenaltyFee { get; set; }
    }
}


