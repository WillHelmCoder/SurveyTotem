using Intelemark.Entities;
using Intelemark.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Intelemark.ScriptServices
{
    public class ReadOnlyContext : IdentityDbContext<ApplicationUser>
    {
        public ReadOnlyContext()
            : base("Special", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = true;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<StateRestriction> StateRestrictions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Entities.TimeZone> TimeZones { get; set; }
        public DbSet<AreaCode> AreaCodes { get; set; }
        public DbSet<UserHistory> UsersHistory { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<CallCode> CallCodes { get; set; }
        public DbSet<AnsweredForm> AnsweredForms { get; set; }
        public DbSet<UserCampaign> UserCampaigns { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<ProjectPriority> ProjectPriorities { get; set; }
        public DbSet<ProjectPriorityDetail> ProjectPriorityDetails { get; set; }
        public DbSet<AgentLog> AgentLogs { get; set; }
        public DbSet<AlertSettings> AlertSettings { get; set; }
        public DbSet<OnWatchSettings> OnWatchSettings { get; set; }
        public DbSet<ExtraField> ExtraFields { get; set; }
       // public DbSet<ExtraFieldOption> ExtraFieldOptions { get; set; }
        public DbSet<CampaignCallCode> CampaignCallCodes { get; set; }
        public DbSet<ExportSettings> ExportSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = true;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }
    }
}