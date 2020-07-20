using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Intelemark.Models;
using Intelemark.Utilities;


namespace Intelemark.Entities
{
    public class CallCode : Entity
    {
        CallCode() { }
        public CallCode(Campaign campaign, string name, string code, EOCBehaviors behavior, bool isSuccess, DateTime now)
        {
            Campaign = campaign ?? throw new Exception("campaign");
            CampaignId = Campaign.Id;

            Name = name;
            Code = code;
            Behavior = behavior;
            IsSuccess = isSuccess;
            
            CreationDate = now;
            LastUpdate = now;

            //campaign.CallCodes.Add(this);
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsSuccess { get; set; }
        public EOCBehaviors Behavior { get; set; }

        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}