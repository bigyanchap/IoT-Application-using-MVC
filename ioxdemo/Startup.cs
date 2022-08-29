using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ioxdemo.Startup))]
namespace ioxdemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
