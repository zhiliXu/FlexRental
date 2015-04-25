using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FlexRental.Startup))]
namespace FlexRental
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
