using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestaurantApplication.Startup))]
namespace RestaurantApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
