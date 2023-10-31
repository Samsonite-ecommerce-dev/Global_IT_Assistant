using Microsoft.Extensions.DependencyInjection;
using Samsonite.Library.Web.Core;

namespace Samsonite.Library.Web.DependencyInjection
{
    public class CoreDI
    {
        public static void Configure(IServiceCollection services)
        {
            //DI 注入
            services.AddScoped<IAppCache, AppCache>();
            services.AddScoped<IAppSession, AppSession>();
            services.AddScoped<IAppCookie, AppCookie>();
            services.AddScoped<IEncryptService, EncryptService>();
            services.AddScoped<IAppNotificationService, AppNotificationService>();
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IFtpService, FtpService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IUploadService, UploadService>();
        }
    }
}
