using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IRMCourseware.Startup))]
namespace IRMCourseware
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
