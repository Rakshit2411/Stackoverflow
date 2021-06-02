using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Stackoverflow.Startup))]
namespace Stackoverflow
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
