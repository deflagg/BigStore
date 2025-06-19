using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BigStore.Startup))]
namespace BigStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
