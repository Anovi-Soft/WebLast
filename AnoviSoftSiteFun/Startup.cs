using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AnoviSoftSiteFun.Startup))]
namespace AnoviSoftSiteFun
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
