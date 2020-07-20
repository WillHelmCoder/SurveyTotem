using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Intelemark.Utilities;


namespace Intelemark.Entities
{
    public class ExportSettings : Entity
    {
        public Int32 CampaignId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}