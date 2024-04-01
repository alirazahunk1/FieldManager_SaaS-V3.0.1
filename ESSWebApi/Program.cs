using ESSWebApi.Extensions;
using ESSWebApi.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:44317")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration();

ConfigurationManager configuration = builder.Configuration;

//Log
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));


builder.Services.ConfigureCors();
builder.Services.ConfigureFCM(configuration);
builder.Services.ConfigureTwilio(configuration);
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureMSSqlContext(builder.Configuration);
//builder.Services.ConfigureMySqlContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureSignalR();
builder.Services.ConfigureRepository();

builder.Services.AddAutoMapper(typeof(Program));

//Addons
builder.Services.AddFeatureDynamicForms();
builder.Services.AddFeatureUidLogin();
builder.Services.AddFeatureDocumentRequest();
builder.Services.AddFeatureProductOrders();
builder.Services.AddFeatureNoticeBoard();
builder.Services.AddFeaturePaymentCollection();
builder.Services.AddFeatureLoanRequest();
builder.Services.AddFeatureBreak();
builder.Services.AddFeatureOfflineTracking();
builder.Services.AddFeatureDynamicQr();
builder.Services.AddFeatureGeofence();
builder.Services.AddFeatureIpAddress();
builder.Services.AddFeatureQrAttendance();
builder.Services.AddFeatureTaskSystem();
//Addons End

//JWT
// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});


builder.Services.AddControllers().AddJsonOptions(opt =>
{

    var enumConverter = new JsonStringEnumConverter();
    opt.JsonSerializerOptions.Converters.Add(enumConverter);
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Field Manager API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
    });
});


var app = builder.Build();

app.UseResponseCompression();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI();
app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/Chat");

app.Run();
