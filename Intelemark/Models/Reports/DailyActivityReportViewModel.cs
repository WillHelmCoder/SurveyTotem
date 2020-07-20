using Intelemark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intelemark.Models.Reports
{
    public class DailyActivityReportViewModel : BaseReportModel
    {
        public string TotalHours { get; set; }
        public Int32 TotalSuccesses { get; set; }

        //Contacts
        public Int32 TotalContacts { get; set; }
        public Int32 ContactsCompleted { get; set; }
        public Int32 ContactsInCallBack { get; set; }
        public Int32 ContactRemaining{ get; set; }
        public Int32 ContactsInHold{ get; set; }

        //DIALS
        public Int32 TotalDials { get; set; }
        public Double DialsPerSuccess{ get; set; }
        public Double DialsPerHour{ get; set; }

        //EOC'S
        public List<CallCodeModel> CallCodes  { get; set; }

        //CHARTS
        public List<Int32> ChartBars { get; set; }


    }
}