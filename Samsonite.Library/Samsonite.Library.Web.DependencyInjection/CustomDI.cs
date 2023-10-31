using Microsoft.Extensions.DependencyInjection;
using Samsonite.Library.Web.Business.Custom;

namespace Samsonite.Library.Web.DependencyInjection
{
    public class CustomDI
    {
        public static void Configure(IServiceCollection services)
        {
            //DI 注入

            //application
            services.AddScoped<IApplicationsService, ApplicationsService>();
            //tool
            services.AddScoped<IOMSAPIToolsService, OMSAPIToolsService>();
        }
    }
}
