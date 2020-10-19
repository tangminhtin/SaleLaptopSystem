using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SaleLaptopSystem.Startup))]
namespace SaleLaptopSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
