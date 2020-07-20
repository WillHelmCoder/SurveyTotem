using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace Intelemark.DTO.DataInventoryByTimeZone
{
    public class Output
    {
        public Detail Detail { set; get; }
        public int Callbacks { set; get; }
        public decimal Penetration { set; get; }

        public List<TimeZone> Data { set; get; } = new List<TimeZone>();

        internal void Update()
        {
            foreach (var timeZone in Data)
                timeZone.Update();

            Detail = new Detail
            {
                Attempt0 = Data.Sum(x => x.Detail.Attempt0),
                Attempt1 = Data.Sum(x => x.Detail.Attempt1),
                Attempt2 = Data.Sum(x => x.Detail.Attempt2),
                Attempt3 = Data.Sum(x => x.Detail.Attempt3),
                Attempt4 = Data.Sum(x => x.Detail.Attempt4),
                Attempt5 = Data.Sum(x => x.Detail.Attempt5),
                Attempt6 = Data.Sum(x => x.Detail.Attempt6),
                MoreThan6 = Data.Sum(x => x.Detail.MoreThan6),
                Final = Data.Sum(x => x.Detail.Final),
                Hold = Data.Sum(x => x.Detail.Hold),
                Total = Data.Sum(x => x.Detail.Total),
            };

            Callbacks += Data.Sum(x => x.Callbacks);

            if (Detail.Total > 0)
                Penetration = Math.Round(((decimal)Detail.Final / Detail.Total) * 100, 2);
        }
    }
    public class TimeZone
    {
        public int Id { set; get; }
        public string Name { set; get; }

        public Detail Detail { set; get; }
        public int Callbacks { set; get; }
        public decimal Penetration { set; get; }


        public List<Project> Data { set; get; } = new List<Project>();

        [JsonIgnore]
        [ScriptIgnore]
        public Dictionary<int, Project> Dictionary { set; get; } = new Dictionary<int, Project>();

        internal void Update()
        {
            Detail = new Detail
            {
                Attempt0 = Data.Sum(x => x.Detail.Attempt0),
                Attempt1 = Data.Sum(x => x.Detail.Attempt1),
                Attempt2 = Data.Sum(x => x.Detail.Attempt2),
                Attempt3 = Data.Sum(x => x.Detail.Attempt3),
                Attempt4 = Data.Sum(x => x.Detail.Attempt4),
                Attempt5 = Data.Sum(x => x.Detail.Attempt5),
                Attempt6 = Data.Sum(x => x.Detail.Attempt6),
                MoreThan6 = Data.Sum(x => x.Detail.MoreThan6),
                Final = Data.Sum(x => x.Detail.Final),
                Hold = Data.Sum(x => x.Detail.Hold),
                Total = Data.Sum(x => x.Detail.Total),
            };

            Callbacks += Data.Sum(x => x.Callbacks);

            if (Detail.Total > 0)
                Penetration = Math.Round(((decimal)Detail.Final / Detail.Total) * 100, 2);

            foreach (var item in Data)
            {
                item.Penetration = item.Detail.Total > 0 ?
                    Math.Round(((decimal)item.Detail.Final / item.Detail.Total) * 100, 2) :
                    0;
            }
        }
    }
    public class Project
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Campaign { set; get; }
        public Detail Detail { set; get; }
        public int Callbacks { set; get; }
        public decimal Penetration { set; get; }
    }
    public class Detail
    {
        public int Attempt0 { set; get; }
        public int Attempt1 { set; get; }
        public int Attempt2 { set; get; }
        public int Attempt3 { set; get; }
        public int Attempt4 { set; get; }
        public int Attempt5 { set; get; }
        public int Attempt6 { set; get; }
        public int MoreThan6 { set; get; }
        public int Final { set; get; }
        public int Hold { set; get; }
        public int Total { set; get; }
    }

    public class Groupping
    {
        public int TimeZoneId { set; get; }
        public int STD { set; get; }
        public int CampaignId { set; get; }
        public int ProjectId { set; get; }
        public Detail Detail { set; get; }
    }
    public class Callback
    {
        public int TimeZoneId { set; get; }
        public int ProjectId { set; get; }
        public int Total { set; get; }
    }
}