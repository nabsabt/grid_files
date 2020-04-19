using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Feladat.Startup))]
namespace Feladat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
