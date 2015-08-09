using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Visualization.Startup))]
namespace Visualization
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
