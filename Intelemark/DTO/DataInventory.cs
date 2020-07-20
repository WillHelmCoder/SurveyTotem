using System;
using System.Collections.Generic;
using System.Linq;

namespace Intelemark.DTO.DataInventory
{
    public class DataInventoryTotals
    {
        public DataInventoryDetail Detail { set; get; }
        public int Callbacks { set; get; }
        public decimal Penetration { set; get; }

        public List<DataInventoryCampaign> Data { set; get; } = new List<DataInventoryCampaign>();

        internal void Update()
        {
            foreach (var campaign in Data)
            {
                foreach (var project in campaign.Data)
                {
                    project.Update();
                }

                campaign.Update();
            }

            Detail = new DataInventoryDetail
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
    public class DataInventoryCampaign
    {
        public int Id { set; get; }
        public string Name { set; get; }

        public DataInventoryDetail Detail { set; get; }
        public int Callbacks { set; get; }
        public decimal Penetration { set; get; }

        public List<DataInventoryProject> Data { set; get; } = new List<DataInventoryProject>();

        internal void Update()
        {
            Detail = new DataInventoryDetail
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
    public class DataInventoryProject
    {
        public int Id { set; get; }
        public string Name { set; get; }

        public DataInventoryDetail Detail { set; get; }
        public int Callbacks { set; get; }
        public decimal Penetration { set; get; }


        public List<DataInventoryRow> Data { set; get; } = new List<DataInventoryRow>();

        internal void Update()
        {
            Detail = new DataInventoryDetail
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

            foreach (var agent in Data)
            {
                agent.Penetration = agent.Detail.Total > 0 ?
                    Math.Round(((decimal)agent.Detail.Final / agent.Detail.Total) * 100, 2) :
                    0;
            }
        }
    }
    public class DataInventoryRow
    {
        public string Name { set; get; }

        public DataInventoryDetail Detail { set; get; }
        public int Callbacks { set; get; }
        public decimal Penetration { set; get; }
    }
    public class DataInventoryDetail
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
}