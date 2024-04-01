using CorePush.Google;
using ESSCommon.Core.Services.Notification;
using ESSCommon.Core.Services.Push;
using ESSCommon.Core.Services.PushNotification;
using ESSCommon.Core.Settings;
using ESSCommon.Core.SharedModels;
using ESSCommon.Core.SMS;
using ESSCommon.Core.Subscription;
using ESSDataAccess.DbContext;
using ESSDataAccess.Identity;
using ESSDataAccess.Tenant;
using ESSWebPortal.ACL;
using ESSWebPortal.Core.Interfaces;
using ESSWebPortal.Core.Plan;
using ESSWebPortal.Core.Services;
using ESSWebPortal.Core.SuperAdmin;
using ESSWebPortal.Logger;
using ESSWebPortal.Middleware;
using ESSWebPortal.Models;
using ESSWebPortal.Razorpay;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ESSWebPortal.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureFCM(this IServiceCollection services, IConfiguration config)
        {
            var section = config.GetSection("FcmSettings");

            var settings = new FcmSettings();
            section.Bind(settings);
            services.AddHttpClient<FcmSender>();

            services.AddSingleton(settings);
        }

        public static void ConfigureTwilio(this IServiceCollection services, IConfiguration config)
        {
            var twilioSection = config.GetSection("Twilio");
            var twilioSettings = new TwilioSettings();
            twilioSection.Bind(twilioSettings);
            services.AddSingleton(twilioSettings);
        }
        public static void ConfigureMobileApp(this IServiceCollection services, IConfiguration config)
        {
            var section = config.GetSection("MobileAppSettings");
            var Settings = new MobileAppSettings();
            section.Bind(Settings);
            services.AddSingleton(Settings);
        }

        public static void ConfigureGoogleMaps(this IServiceCollection services, IConfiguration config)
        {
            var section = config.GetSection("GoogleMaps");
            var maps = new GoogleMaps();
            section.Bind(maps);
            services.AddSingleton(maps);
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();

        }


        public static void ConfigureMSSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("Default");

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString,
                    x => x.MigrationsAssembly(nameof(ESSDataAccess)));
            });
        }

        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("MySqlConnection");

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {


        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireDigit = false;
                opt.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

        }

        public static void ConfigureAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {

                options.AddPolicy("superadmin",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("superadmin");
                    });

            });
        }

        public static void ConfigureSignalR(this IServiceCollection services)
        {
            services.AddSignalR();
        }

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddTransient<IReport, ReportService>();
            services.AddTransient<ISMS, SMSService>();
            services.AddTransient<IDbSettings, DbSettingsService>();
            services.AddTransient<ISASettings, SASettingsService>();
            services.AddTransient<IPlan, PlanService>();

            services.AddSingleton<IPush, PushService>();
            services.AddTransient<IPushNotification, PushNotificationService>();
            services.AddTransient<INotification, NotificationService>();

            services.AddTransient<ISuperAdmin, SuperAdminService>();
            services.AddSingleton<ITenant, TenantService>();

            services.AddSingleton<IMvcControllerDiscovery, MvcControllerDiscovery>();

            services.AddTransient<ISubscription, SubscriptionService>();


            services.AddTransient<IRazorpay, RazorPayService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void RegisterMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<MultiTenantServiceMiddleware>();
        }

        public static void ConfigureCookies(this IServiceCollection services)
        {
            // Cookies and Account Routes
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Auth/Login";
                options.LogoutPath = $"/Auth/Logout";
                options.AccessDeniedPath = $"/Auth/AccessDenied";
            });
        }
    }
}
