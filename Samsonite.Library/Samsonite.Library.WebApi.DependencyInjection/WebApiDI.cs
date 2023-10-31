using Microsoft.Extensions.DependencyInjection;
using Samsonite.Library.WebApi.Bussness;
using Samsonite.Library.WebApi.Core;

namespace Samsonite.Library.WebApi.DependencyInjection
{
    public class WebApiDI
    {
        public static void Configure(IServiceCollection services)
        {
            //core
            services.AddScoped<IApiBaseService, ApiBaseService>();
            services.AddScoped<IAuthorizeService, AuthorizeService>();
            services.AddScoped<IMenuService, MenuService>();
            //api
        }
    }
}
