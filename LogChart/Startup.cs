using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LogChart.Startup))]
namespace LogChart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app) { }
    }
}
