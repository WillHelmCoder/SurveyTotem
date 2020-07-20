using Microsoft.Owin;
using System.Threading.Tasks;
using Owin;

[assembly: OwinStartup(typeof(Intelemark.Startup))]
namespace Intelemark
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();

            Services.Hangfire.ConfigureHangfire(app);
            Services.Hangfire.InitializeJobs();
        }
    }
}
