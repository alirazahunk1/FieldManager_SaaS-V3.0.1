using ESSWebPortal.ACL;
using ESSWebPortal.Extensions;
using ESSWebPortal.Hubs;
using ESSWebPortal.Middleware;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureMSSqlContext(builder.Configuration);
//builder.Services.ConfigureMySqlContext(builder.Configuration);
builder.Services.ConfigureFCM(builder.Configuration);
builder.Services.ConfigureTwilio(builder.Configuration);
builder.Services.ConfigureMobileApp(builder.Configuration);
builder.Services.ConfigureGoogleMaps(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureRepositoryWrapper();
builder.Services.ConfigureRepository();
builder.Services.ConfigureCookies();
builder.Services.RegisterMiddlewares();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddRazorPages();
builder.Services.ConfigureAuthorizationPolicy();

//Core Extensions
builder.Services.ConfigureSuperAdmin(builder.Configuration);

//Addons configuration
builder.Services.AddFeatureDynamicForms(builder.Environment);
builder.Services.AddFeatureLoan();
builder.Services.AddFeatureNoticeBoard();
builder.Services.AddFeatureProductOrder();
builder.Services.AddFeaturePaymentCollection();
builder.Services.AddFeatureDataImportExport();
builder.Services.AddFeatureDocumentRequest();
builder.Services.AddFeatureGeofence();
builder.Services.AddFeatureIpAddress();
builder.Services.AddFeatureSite();
builder.Services.AddFeatureQrCode();
builder.Services.AddFeatureDynamicQrCode();
builder.Services.AddFeatureTaskSystem();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(DynamicAuthorizationFilter));
}).AddSessionStateTempDataProvider()
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        ProgressBar = true,
        CloseButton = true,
        TapToDismiss = true,
        PositionClass = ToastPositions.BottomRight
    });
builder.Services.AddSession();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.ConfigureSignalR();

builder.Configuration.AddJsonFile("sidebar.json",
    optional: true,
    reloadOnChange: true);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<MultiTenantServiceMiddleware>();
app.UseRouting();
app.UseSession();

app.UseAuthentication();

app.UseAuthorization();


app.UseNToastNotify();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.MapHub<CardViewHub>("/cardView");

app.Run();
