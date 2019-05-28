using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PeliculasEdwin.Startup))]
namespace PeliculasEdwin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}