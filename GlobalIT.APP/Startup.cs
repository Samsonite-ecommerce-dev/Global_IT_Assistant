using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Web.Core;
using Samsonite.Library.Web.DependencyInjection;
using Samsonite.Library.WebApi.DependencyInjection;

namespace GlobalIT.APP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //������ϵע�����ݿ�������Ϣ
            services.AddDbContext<appEntities>(options => options.UseSqlServer(Configuration.GetConnectionString("appConnection")));
            services.AddDbContext<logEntities>(options => options.UseSqlServer(Configuration.GetConnectionString("logConnection")));

            //���HttpContext
            services.AddHttpContextAccessor();
            //���session
            services.AddSession();
            services.AddDataProtection();
            //�Զ���DIע��
            CoreDI.Configure(services);
            BasicDI.Configure(services);
            CustomDI.Configure(services);
            WebApiDI.Configure(services);
            //ȫ�ֹ�����ע��
            GlobalFilterDI.Configure(services);
            ////�Զ���Antiforgery
            ///MVC���Զ���<Form>��ǩ��������__RequestVerificationToken
            //services.AddAntiforgery(option =>
            //{

            //});

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    //���ص�JSON����NULL
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBaseService baseService)
        {
            //����վ�������ļ�
            baseService.LoadConfigCache();
            //����վ�����԰�
            baseService.LoadLanguagePackCache();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //����session
            app.UseSession();
            //���þ�̬�ļ��м��(wwwĿ¼)
            app.UseStaticFiles();
            ////�Զ���UploadFileĿ¼
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "UploadFile")),
            //    RequestPath = "/UploadFile"
            //});
            app.UseRouting();
            //app.UseCookiePolicy();
            //app.UseAuthorization();
            //�������ҳ��
            //ע:{0}���ص���Status code(404,500��)
            app.UseStatusCodePagesWithRedirects("/Error?StatusCode={0}");
            //·������
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
