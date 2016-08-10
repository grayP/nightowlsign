using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(nightowlsigns.Startup))]
namespace nightowlsigns
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
