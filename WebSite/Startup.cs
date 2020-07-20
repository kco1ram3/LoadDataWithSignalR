using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(WebSite.Startup))]

namespace WebSite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}