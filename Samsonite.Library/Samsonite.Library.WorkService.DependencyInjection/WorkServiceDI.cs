using Microsoft.Extensions.DependencyInjection;
using Samsonite.Library.Web.Core;
using Samsonite.Library.WorkService.Core;

namespace Samsonite.Library.WorkService.DependencyInjection
{
    public class WorkServiceDI
    {
        public static void Configure(IServiceCollection services)
        {
            //core
            services.AddScoped<IApplicationService, ApplicationService>();
            //service
            //related
            services.AddScoped<IFtpService, FtpService>();
        }
    }
}
