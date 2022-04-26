using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShoolsLMS.Startup))]
namespace ShoolsLMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
