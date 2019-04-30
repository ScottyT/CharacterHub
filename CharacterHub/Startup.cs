using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CharacterHub.Startup))]
namespace CharacterHub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
