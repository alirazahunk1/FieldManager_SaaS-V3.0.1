using ESSWebPortal.AddonHelpers;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.FileProviders;

namespace ESSWebPortal.Extensions
{
    public static class CoreExtensions
    {
        public static void ConfigureSuperAdmin(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ApiModuleHelper>();

            var assembly = typeof(CZ.WebCore.SuperAdmin.Controllers.SuperAdminController).Assembly;

            services.AddControllersWithViews()
                .AddApplicationPart(assembly)
                .AddRazorRuntimeCompilation()
                .AddControllersAsServices();

            services.Configure<MvcRazorRuntimeCompilationOptions>(option =>
            {
                option.FileProviders.Add(new EmbeddedFileProvider(assembly));
            });

            var assembly2 = typeof(CZ.WebCore.SuperAdmin.Controllers.PlanController).Assembly;

            services.AddControllersWithViews()
                .AddApplicationPart(assembly2)
                .AddRazorRuntimeCompilation()
                .AddControllersAsServices();

            services.Configure<MvcRazorRuntimeCompilationOptions>(option =>
            {
                option.FileProviders.Add(new EmbeddedFileProvider(assembly2));
            });


            var assembly3 = typeof(CZ.WebCore.SuperAdmin.Controllers.SASettingsController).Assembly;

            services.AddControllersWithViews()
                .AddApplicationPart(assembly3)
                .AddRazorRuntimeCompilation()
                .AddControllersAsServices();

            services.Configure<MvcRazorRuntimeCompilationOptions>(option =>
            {
                option.FileProviders.Add(new EmbeddedFileProvider(assembly3));
            });




        }
    }
}
