﻿using Azure.Identity;
using Birder.MinApiEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SendGrid.Extensions.DependencyInjection;


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

builder.Services.AddProblemDetails();

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

builder.Services.Configure<ConfigOptions>(builder.Configuration.GetSection(ConfigOptions.Config));

var autoMapper =  builder.Configuration.GetRequiredSection("AutoMapper").Get<AutoMapperOptions>();
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = autoMapper.License, typeof(BirderMappingProfile));

builder.Services.AddHttpClient();

RegisterCustomServices(builder);

builder.Services.Configure<FlickrOptions>(builder.Configuration.GetSection(FlickrOptions.Flickr));
builder.Services.Configure<ConfigOptions>(builder.Configuration.GetSection(ConfigOptions.Config));

var authConfig = builder.Configuration.GetRequiredSection("Config").Get<ConfigOptions>();
builder.Services.AddSendGrid(options => { options.ApiKey = authConfig.SendGridKey; options.HttpErrorAsException = true; });

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

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(MyAllowSpecificOrigins,
//        builder =>
//        {
//            builder.WithOrigins(authConfig.BaseUrl);
//        });
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://andrewstuartcross.co.uk",
                            "https://www.andrewstuartcross.co.uk")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                      });
});


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

app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context
        => await Results.Problem()
                     .ExecuteAsync(context)));

app.UseStatusCodePages(async statusCodeContext
   => await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
                .ExecuteAsync(statusCodeContext.HttpContext));

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", GeneralEndpoints.ServerInfo);

app.MapGet("/api/birds-list", BirdEndpoints.GetBirdsDdlAsync)
   .RequireAuthorization();

var birdsGroup = app.MapGroup("api/birds");

birdsGroup.MapGet("", BirdEndpoints.GetBirdsAsync)
    .RequireAuthorization();

birdsGroup.MapGet("/bird", BirdEndpoints.GetBirdAsync)
    .RequireAuthorization();

var listGroup = app.MapGroup("api/list");

listGroup.MapGet("/top", ListEndpoints.GetTopObservationsAsync)
    .RequireAuthorization();

listGroup.MapGet("/top-date-filter", ListEndpoints.GetTopObservationsWithDateFilterAsync)
    .RequireAuthorization();

listGroup.MapGet("/life", ListEndpoints.GetLifeListAsync)
    .RequireAuthorization();


app.Run();


static void RegisterCustomServices(WebApplicationBuilder builder)
{
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
    builder.Services.AddScoped<ICachedBirdsDdlService, CachedBirdsDdlService>();

    builder.Services.AddSingleton<ISystemClockService, SystemClockService>();
    builder.Services.AddSingleton<IUrlService, UrlService>();
    builder.Services.AddSingleton<IUserNetworkHelpers, UserNetworkHelpers>();

    builder.Services.AddTransient<IEmailSender, EmailSender>();
}

[ExcludeFromCodeCoverageAttribute]
public partial class Program { }