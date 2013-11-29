using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Julekalender.Startup))]
namespace Julekalender
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
