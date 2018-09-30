using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InvoicingApp.Web.Startup))]
namespace InvoicingApp.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
