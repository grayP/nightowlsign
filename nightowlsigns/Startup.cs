using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(nightowlsign.Startup))]
namespace nightowlsign
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
