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
using ESSWebApi.JWT;
using ESSWebApi.Logger;
using ESSWebApi.Services.Auth;
using ESSWebApi.Services.Client;
using ESSWebApi.Services.Dashboard;
using ESSWebApi.Services.Device;
using ESSWebApi.Services.Expense;
using ESSWebApi.Services.Leave;
using ESSWebApi.Services.Manager;
using ESSWebApi.Services.Schedule;
using ESSWebApi.Services.Settings;
using ESSWebApi.Services.Visit;
using ESSWebPortal.Core.SuperAdmin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Extensions
{
    public static class ServiceExtensions
    {
        //Test
        public static void ConfigureFCM(this IServiceCollection services, IConfiguration config)
        {
            var section = config.GetSection("FcmSettings");
            var settings = new FcmSettings();
            section.Bind(settings);
            services.AddHttpClient<FcmSender>();

            services.AddSingleton(settings);

        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureTwilio(this IServiceCollection services, IConfiguration config)
        {
            var twilioSection = config.GetSection("Twilio");
            var twilioSettings = new TwilioSettings();
            twilioSection.Bind(twilioSettings);
            services.AddSingleton(twilioSettings);
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
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
        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped<JWTManager>();
            services.AddTransient<IAuthentication, AuthenticationRepo>();
            services.AddTransient<IScheduleManager, ScheduleRepo>();
            services.AddTransient<IDevice, DeviceRepo>();
            services.AddTransient<IUser, UserRepo>();
            services.AddTransient<ILeave, LeaveRepo>();
            services.AddTransient<IExpense, ExpenseService>();
            services.AddTransient<IClient, ClientService>();
            services.AddTransient<ISMS, SMSService>();
            services.AddTransient<ISettings, SettingsService>();
            services.AddTransient<IVisit, VisitService>();

            services.AddTransient<INotification, NotificationService>();
            services.AddTransient<ESSWebApi.Services.Notification.INotification, ESSWebApi.Services.Notification.NotificationRepo>();
            services.AddSingleton<IPush, PushService>();
            services.AddTransient<IPushNotification, PushNotificationService>();
            services.AddTransient<IDbSettings, DbSettingsService>();
            services.AddTransient<ISASettings, SASettingsService>();


            services.AddTransient<ISubscription, SubscriptionService>();

            services.AddSingleton<ITenant, TenantService>();

            //Manager APP
            services.AddTransient<IManager, ManagerRepo>();

        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        }

        public static void ConfigureSignalR(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
            });
        }

    }
}
