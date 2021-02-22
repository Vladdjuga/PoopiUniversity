using Microsoft.Owin;
using Owin;
using poopi.Helpers;

[assembly: OwinStartupAttribute(typeof(poopi.Startup))]
namespace poopi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            SeederDatabase.SeedData();
        }
    }
}
