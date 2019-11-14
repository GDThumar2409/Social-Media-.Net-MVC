using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PROJECT.Startup))]
namespace PROJECT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
            
        }
    }
}
