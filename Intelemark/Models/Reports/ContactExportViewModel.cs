using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intelemark.Models.Reports
{
    public class ContactExportViewModel
    {
        public Campaign SelectedCampaign { get; set; }
        public Int32 CampaignId { get; set; }
        public Int32 ExportSettingsId { get; set; }
        public virtual List<ContactModel> Contacts { get; set; }
        public List<Question> Forms { get; set; }

    }
}