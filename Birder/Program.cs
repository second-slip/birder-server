using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
        new DefaultAzureCredential());
}

builder.Services.AddMemoryCache();

builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.ConfigureSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "birder-server",
        Version = "v1"
    });
});

var connectionStrings = builder.Configuration.GetRequiredSection("ConnectionStrings").Get<ConnectionStringsOptions>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(connectionStrings.DefaultConnection));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 2;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        .AddSignInManager<SignInManager<ApplicationUser>>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient();

// custom services
// move to separate method for clarity?  AddCustomServices(WebApplicationBuilder builder)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBirdRepository, BirdRepository>();
builder.Services.AddScoped<IObservationRepository, ObservationRepository>();
builder.Services.AddScoped<IObservationPositionRepository, ObservationPositionRepository>();
builder.Services.AddScoped<IObservationNoteRepository, ObservationNoteRepository>();
builder.Services.AddScoped<INetworkRepository, NetworkRepository>();
builder.Services.AddScoped<IServerlessDatabaseService, ServerlessDatabaseService>();
builder.Services.AddScoped<IListService, ListService>();
builder.Services.AddScoped<IObservationQueryService, ObservationQueryService>();
builder.Services.AddScoped<IBirdDataService, BirdDataService>();
builder.Services.AddScoped<ITweetDataService, TweetDataService>();
builder.Services.AddScoped<IObservationsAnalysisService, ObservationsAnalysisService>();
builder.Services.AddScoped<IFlickrService, FlickrService>();
builder.Services.AddScoped<IBirdThumbnailPhotoService, BirdThumbnailPhotoService>();
builder.Services.AddScoped<IAuthenticationTokenService, AuthenticationTokenService>();
builder.Services.AddScoped<IXenoCantoService, XenoCantoService>();

builder.Services.AddSingleton<ISystemClockService, SystemClockService>();
builder.Services.AddSingleton<IUrlService, UrlService>();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.Configure<FlickrOptions>(builder.Configuration.GetSection(FlickrOptions.Flickr));
builder.Services.Configure<AuthConfigOptions>(builder.Configuration.GetSection(AuthConfigOptions.AuthConfig));


// fault
var authConfig = builder.Configuration.GetRequiredSection("AuthConfig").Get<AuthConfigOptions>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authConfig.BaseUrl,
            ValidAudience = authConfig.BaseUrl,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.TokenKey))
        };
    });

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(MyAllowSpecificOrigins,
//        builder =>
//        {
//            builder.WithOrigins(authConfig.BaseUrl);
//        });
//});
// services.AddAzureClients(builder =>
// {
//     builder.AddBlobServiceClient(Configuration["ConnectionStrings:StorageConnection:blob"], preferMsi: true);
//     builder.AddQueueServiceClient(Configuration["ConnectionStrings:StorageConnection:queue"], preferMsi: true);
// });

//builder.WebHost.ConfigureLogging(ConfigLogging); // not sure about this


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "birder-server v1"));
}

app.UseHttpsRedirection();

// app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", (ISystemClockService date) =>
    string.Join(
        Environment.NewLine,
        "birder-server API",
        "https://github.com/winthorpecross/birder-server",
        $"{date.GetNow}",
        $"\u00A9 Birder {date.GetNow.Year}"));

app.Run();

[ExcludeFromCodeCoverageAttribute]
public partial class Program { }