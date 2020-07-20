using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intelemark.Models
{
    public class AgentModel : Model
    {
        public String Name { get; set; }
        public List<Contact> Contacts { get; set; }
        public ContactModel CurrentContact { get; set; }
        public FormModel Form { get; set; }
        public RecordModel Record { get; set; }
        public CampaignModel CurrentCampaign { get; set; }
        public AppointmentModel Appointment { get; set; }
        public int CallbackId { get; set; }
        public List<AnsweredForm> FormAnswers { get; set; }
        public List<AnsweredFormModel> FormAnswersModel { get; set; }
    }
}