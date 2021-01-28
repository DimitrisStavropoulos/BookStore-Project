using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Store01.Startup))]
namespace Store01
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
