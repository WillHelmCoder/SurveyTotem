using Intelemark.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intelemark.DTO.CallCodes
{
    public class Output
    {
        public int CallCodeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DropDownDisplay { get => $"{Code} - {Name}"; }
        public EOCBehaviors Behavior { get; set; }
        public bool IsSuccess { get; set; }

        public int CampaignId { get; set; }
        public string CampaignDescription { get; set; }
        public string CampaignIdentifier { get; set; }

        public int Count { get; set; }
    }
}