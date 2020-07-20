namespace Intelemark.BigMigration
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class IntelemarkOldModel : DbContext
    {
        public IntelemarkOldModel()
            : base("name=IntelemarkOldModel")
        {
        }

        public virtual DbSet<AgentLeadTracking> AgentLeadTrackings { get; set; }
        public virtual DbSet<dtproperty> dtproperties { get; set; }
        public virtual DbSet<elMaster> elMasters { get; set; }
        public virtual DbSet<eMoreUDF> eMoreUDFs { get; set; }
        public virtual DbSet<etAgent> etAgents { get; set; }
        public virtual DbSet<etAgentCampaign> etAgentCampaigns { get; set; }
        public virtual DbSet<etAgentIncentive> etAgentIncentives { get; set; }
        public virtual DbSet<etCallBack> etCallBacks { get; set; }
        public virtual DbSet<etCampaign> etCampaigns { get; set; }
        public virtual DbSet<etCityStateZip> etCityStateZips { get; set; }
        public virtual DbSet<etEOC> etEOCs { get; set; }
        public virtual DbSet<etLoad> etLoads { get; set; }
        public virtual DbSet<etLoadMore> etLoadMores { get; set; }
        public virtual DbSet<etLookup> etLookups { get; set; }
        public virtual DbSet<etSession> etSessions { get; set; }
        public virtual DbSet<etSFI> etSFIs { get; set; }
        public virtual DbSet<eUDF> eUDFs { get; set; }
        public virtual DbSet<ExportFMT> ExportFMTs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Deliverable> Deliverables { get; set; }
        public virtual DbSet<etAgentMaster> etAgentMasters { get; set; }
        public virtual DbSet<etCallBackBackup> etCallBackBackups { get; set; }
        public virtual DbSet<etCallHistory> etCallHistories { get; set; }
        public virtual DbSet<etLoadSummary> etLoadSummaries { get; set; }
        public virtual DbSet<etMessage> etMessages { get; set; }
        public virtual DbSet<eUDFMore> eUDFMores { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgentLeadTracking>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<AgentLeadTracking>()
                .Property(e => e.AgentId)
                .IsUnicode(false);

            modelBuilder.Entity<dtproperty>()
                .Property(e => e.property)
                .IsUnicode(false);

            modelBuilder.Entity<dtproperty>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Contact)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Company)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Address1)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Address2)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Zip)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Phone1)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.PhoneExt)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.DirLine)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Mobile)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.FaxNumber)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.WebURL)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.BusType)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.SalesVol)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.SIC)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.TimeZone)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.ProjectId)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.AgentId)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.EOC)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF1)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF2)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF3)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF4)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF5)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF6)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF7)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF8)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF9)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF10)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF11)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF12)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF13)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF14)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF15)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.UDF16)
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.DataGroup)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Source)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.IsActive)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.RecordLock)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<elMaster>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<eMoreUDF>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<eMoreUDF>()
                .Property(e => e.UDFNum)
                .IsUnicode(false);

            modelBuilder.Entity<eMoreUDF>()
                .Property(e => e.Label)
                .IsUnicode(false);

            modelBuilder.Entity<eMoreUDF>()
                .Property(e => e.Format)
                .IsUnicode(false);

            modelBuilder.Entity<eMoreUDF>()
                .Property(e => e.Mask)
                .IsUnicode(false);

            modelBuilder.Entity<eMoreUDF>()
                .Property(e => e.DefaultValue)
                .IsUnicode(false);

            modelBuilder.Entity<eMoreUDF>()
                .Property(e => e.SpellUDF)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etAgent>()
                .Property(e => e.AgentID)
                .IsUnicode(false);

            modelBuilder.Entity<etAgent>()
                .Property(e => e.CampaignID)
                .IsUnicode(false);

            modelBuilder.Entity<etAgent>()
                .Property(e => e.ProjectId)
                .IsFixedLength();

            modelBuilder.Entity<etAgent>()
                .Property(e => e.TimeZone)
                .IsFixedLength();

            modelBuilder.Entity<etAgent>()
                .Property(e => e.IsActive)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etAgent>()
                .Property(e => e.Client_Timezone)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentCampaign>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentCampaign>()
                .Property(e => e.AgentID)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentCampaign>()
                .Property(e => e.CampaignHours)
                .HasPrecision(18, 0);

            modelBuilder.Entity<etAgentCampaign>()
                .Property(e => e.DialMode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etAgentCampaign>()
                .Property(e => e.PayPerHour)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etAgentCampaign>()
                .Property(e => e.PayPerSale)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etAgentCampaign>()
                .Property(e => e.PayPerTrnHour)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etAgentCampaign>()
                .Property(e => e.IncentiveID1)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentCampaign>()
                .Property(e => e.IncentiveID3)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentCampaign>()
                .Property(e => e.IncentiveID2)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentCampaign>()
                .Property(e => e.IsActive)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.IncentiveID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.EOC)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.IncentiveType)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.PayMethod)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.MeasureLevel1)
                .HasPrecision(18, 3);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.MeasureLevel2)
                .HasPrecision(18, 3);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.MeasureLevel3)
                .HasPrecision(18, 3);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.MeasureLevel4)
                .HasPrecision(18, 3);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.MeasureLevel5)
                .HasPrecision(18, 3);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.MeasureLevel6)
                .HasPrecision(18, 3);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.PayLevel1)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.PayLevel2)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.PayLevel3)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.PayLevel4)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.PayLevel5)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etAgentIncentive>()
                .Property(e => e.PayLevel6)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etCallBack>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBack>()
                .Property(e => e.AgentID)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBack>()
                .Property(e => e.Status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCallBack>()
                .Property(e => e.Contact)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBack>()
                .Property(e => e.Phone1)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBack>()
                .Property(e => e.CallbackDate01)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBack>()
                .Property(e => e.CallbackTime01)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBack>()
                .Property(e => e.Comments)
                .IsFixedLength();

            modelBuilder.Entity<etCallBack>()
                .Property(e => e.Reminded)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.Campaign)
                .IsFixedLength();

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.CampaignDesc)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.CampaignObjective)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.Link1)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.Link1Label)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.Link2)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.Link2Label)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.Link3)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.Link3Label)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.DailyHours)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.TimezoneUpdateFlag)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.BillingPerHourProduction)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.BillingPerHourTraining)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.BillingPerSale)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.CampaignSupervisor)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.IsActive)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.MaxQueue)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.MaxAttempts)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.OnWatch)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.CPhone)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.Client_Number)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.QueueSortBy)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.CompanyLink)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.SpellNotes)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.DailyActivityReportPath)
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .Property(e => e.HoursTracking)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCampaign>()
                .HasMany(e => e.etCallBacks)
                .WithRequired(e => e.etCampaign)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<etCampaign>()
                .HasMany(e => e.etLookups)
                .WithRequired(e => e.etCampaign)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<etCampaign>()
                .HasMany(e => e.eUDFs)
                .WithRequired(e => e.etCampaign)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<etCityStateZip>()
                .Property(e => e.Zip)
                .IsFixedLength();

            modelBuilder.Entity<etCityStateZip>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<etCityStateZip>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<etCityStateZip>()
                .Property(e => e.Timezone)
                .IsUnicode(false);

            modelBuilder.Entity<etEOC>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<etEOC>()
                .Property(e => e.EOC)
                .IsUnicode(false);

            modelBuilder.Entity<etEOC>()
                .Property(e => e.EOCDesc)
                .IsUnicode(false);

            modelBuilder.Entity<etEOC>()
                .Property(e => e.Recyclable)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etEOC>()
                .Property(e => e.Actions)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etEOC>()
                .Property(e => e.Display)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etEOC>()
                .Property(e => e.IsActive)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etEOC>()
                .Property(e => e.Type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etEOC>()
                .Property(e => e.InSPH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etEOC>()
                .Property(e => e.DMPrompt)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Contact)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Company)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Address1)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Address2)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Zip)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Phone1)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.PhoneExt)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.DirLine)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Mobile)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.FaxNumber)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.WebURL)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.BusType)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.SalesVol)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.SIC)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.TimeZone)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.ProjectId)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.AgentId)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.EOC)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF1)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF2)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF3)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF4)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF5)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF6)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF7)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF8)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF9)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF10)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF11)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF12)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF13)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF14)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF15)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.UDF16)
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.DataGroup)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Source)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etLoad>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF17)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF18)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF19)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF20)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF21)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF22)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF23)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF24)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF25)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF26)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF27)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF28)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF29)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF30)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF31)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.UDF32)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.ProjectId)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.DataGroup)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etLoadMore>()
                .Property(e => e.Source)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etLookup>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<etLookup>()
                .Property(e => e.Tablename)
                .IsUnicode(false);

            modelBuilder.Entity<etLookup>()
                .Property(e => e.TValue)
                .IsUnicode(false);

            modelBuilder.Entity<etSession>()
                .Property(e => e.CampaignID)
                .IsUnicode(false);

            modelBuilder.Entity<etSession>()
                .Property(e => e.AgentID)
                .IsUnicode(false);

            modelBuilder.Entity<etSession>()
                .Property(e => e.PayHourly)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etSession>()
                .Property(e => e.PaySales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etSession>()
                .Property(e => e.PayIncentives)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etSession>()
                .Property(e => e.PayOther)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etSession>()
                .Property(e => e.PayNotes)
                .IsUnicode(false);

            modelBuilder.Entity<etSession>()
                .Property(e => e.RevenueHourly)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etSession>()
                .Property(e => e.RevenuePerSales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<etSession>()
                .Property(e => e.Active)
                .IsUnicode(false);

            modelBuilder.Entity<etSFI>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<etSFI>()
                .Property(e => e.SFUserID)
                .IsUnicode(false);

            modelBuilder.Entity<etSFI>()
                .Property(e => e.SFPassword)
                .IsUnicode(false);

            modelBuilder.Entity<etSFI>()
                .Property(e => e.SFToken)
                .IsUnicode(false);

            modelBuilder.Entity<etSFI>()
                .Property(e => e.SFLeadSource)
                .IsUnicode(false);

            modelBuilder.Entity<eUDF>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<eUDF>()
                .Property(e => e.UDFNum)
                .IsUnicode(false);

            modelBuilder.Entity<eUDF>()
                .Property(e => e.Label)
                .IsUnicode(false);

            modelBuilder.Entity<eUDF>()
                .Property(e => e.Format)
                .IsUnicode(false);

            modelBuilder.Entity<eUDF>()
                .Property(e => e.Mask)
                .IsUnicode(false);

            modelBuilder.Entity<eUDF>()
                .Property(e => e.DefaultValue)
                .IsUnicode(false);

            modelBuilder.Entity<eUDF>()
                .Property(e => e.SpellUDF)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ExportFMT>()
                .Property(e => e.ExportName)
                .IsUnicode(false);

            modelBuilder.Entity<ExportFMT>()
                .Property(e => e.CampaignID)
                .IsUnicode(false);

            modelBuilder.Entity<ExportFMT>()
                .Property(e => e.ExportField)
                .IsUnicode(false);

            modelBuilder.Entity<ExportFMT>()
                .Property(e => e.LabelName)
                .IsUnicode(false);

            modelBuilder.Entity<Deliverable>()
                .Property(e => e.Campaignid)
                .IsUnicode(false);

            modelBuilder.Entity<Deliverable>()
                .Property(e => e.EOC)
                .IsUnicode(false);

            modelBuilder.Entity<Deliverable>()
                .Property(e => e.Layout)
                .IsUnicode(false);

            modelBuilder.Entity<Deliverable>()
                .Property(e => e.ExportFormat)
                .IsUnicode(false);

            modelBuilder.Entity<Deliverable>()
                .Property(e => e.TransDirectory)
                .IsUnicode(false);

            modelBuilder.Entity<Deliverable>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<Deliverable>()
                .Property(e => e.FieldsChanged)
                .IsUnicode(false);

            modelBuilder.Entity<Deliverable>()
                .Property(e => e.Char)
                .IsUnicode(false);

            modelBuilder.Entity<Deliverable>()
                .Property(e => e.QC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.AgentID)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.AgentName)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.Zip)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.Cell)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.PayMethod)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.SecurityLevel)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.AgentPassword)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.TotalHours)
                .HasPrecision(18, 0);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.IsActive)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.AgentLevel)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.Experience)
                .IsUnicode(false);

            modelBuilder.Entity<etAgentMaster>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBackBackup>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBackBackup>()
                .Property(e => e.AgentID)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBackBackup>()
                .Property(e => e.Status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCallBackBackup>()
                .Property(e => e.Contact)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBackBackup>()
                .Property(e => e.Phone1)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBackBackup>()
                .Property(e => e.CallbackDate01)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBackBackup>()
                .Property(e => e.CallbackTime01)
                .IsUnicode(false);

            modelBuilder.Entity<etCallBackBackup>()
                .Property(e => e.Comments)
                .IsFixedLength();

            modelBuilder.Entity<etCallBackBackup>()
                .Property(e => e.Reminded)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCallHistory>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<etCallHistory>()
                .Property(e => e.AgentID)
                .IsUnicode(false);

            modelBuilder.Entity<etCallHistory>()
                .Property(e => e.ProjectId)
                .IsUnicode(false);

            modelBuilder.Entity<etCallHistory>()
                .Property(e => e.CallType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCallHistory>()
                .Property(e => e.DecisionMaker)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etCallHistory>()
                .Property(e => e.Presentation)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etLoadSummary>()
                .Property(e => e.DataGroup)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etLoadSummary>()
                .Property(e => e.Campaignid)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadSummary>()
                .Property(e => e.Projectid)
                .IsUnicode(false);

            modelBuilder.Entity<etLoadSummary>()
                .Property(e => e.Source)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<etLoadSummary>()
                .Property(e => e.Load_Notes)
                .IsUnicode(false);

            modelBuilder.Entity<etMessage>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<etMessage>()
                .Property(e => e.AgentId)
                .IsUnicode(false);

            modelBuilder.Entity<etMessage>()
                .Property(e => e.Heading)
                .IsUnicode(false);

            modelBuilder.Entity<etMessage>()
                .Property(e => e.Note)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.CampaignId)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF17)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF18)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF19)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF20)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF21)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF22)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF23)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF24)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF25)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF26)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF27)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF28)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF29)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF30)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF31)
                .IsUnicode(false);

            modelBuilder.Entity<eUDFMore>()
                .Property(e => e.UDF32)
                .IsUnicode(false);
        }
    }
}
