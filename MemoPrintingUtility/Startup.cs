using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MemoPrintingUtility.Startup))]
namespace MemoPrintingUtility
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
