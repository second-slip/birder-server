



//using Azure.Core.Extensions;
//using Azure.Storage.Blobs;
//using Azure.Storage.Queues;
//using Birder;
//using Birder.Data;
//using Birder.Data.Model;
//using Birder.Data.Repository;
//using Birder.Services;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.SpaServices.AngularCli;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Azure;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.Tokens;
//using Newtonsoft.Json;
//using System;
//using System.Text;




//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();


////var configuration = builder.Configuration.GetValue<String>(""); // provider.GetRequiredService<IConfiguration>();


//app.MapGet("/", () => "Hello World!"); // routung go here

//// Services
//builder.Services.AddMemoryCache();

//builder.Services.AddControllers().AddNewtonsoftJson(options =>
//{
//    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
//    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
//});

//builder.Services.AddSpaStaticFiles(configuration =>
//{
//    configuration.RootPath = "ClientApp/dist";
//});

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//      options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrings:DefaultConnection")));

//builder.Services.AddIdentityCore<ApplicationUser>(options =>
//{
//    options.SignIn.RequireConfirmedEmail = true;
//    options.User.RequireUniqueEmail = true;
//    // Password settings: require any eight letters or numbers
//    options.Password.RequireDigit = true;
//    options.Password.RequiredLength = 8;
//    options.Password.RequiredUniqueChars = 2;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireUppercase = false;
//})
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders()
//    .AddSignInManager<SignInManager<ApplicationUser>>();

//builder.Services.AddAutoMapper((serviceProvider, automapper) =>
//{
//    automapper.AddCollectionMappers();
//    //automapper.UseEntityFrameworkCoreModel<ApplicationDbContext>(serviceProvider);
//}, typeof(ApplicationDbContext).Assembly);


//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<IBirdRepository, BirdRepository>();
//builder.Services.AddScoped<IObservationRepository, ObservationRepository>();
//builder.Services.AddScoped<IObservationPositionRepository, ObservationPositionRepository>();
//builder.Services.AddScoped<IObservationNoteRepository, ObservationNoteRepository>();
//builder.Services.AddScoped<INetworkRepository, NetworkRepository>();
//builder.Services.AddScoped<ITweetDayRepository, TweetDayRepository>();

//builder.Services.AddScoped<IServerlessDatabaseService, ServerlessDatabaseService>();
//builder.Services.AddScoped<IListService, ListService>();
//builder.Services.AddScoped<IObservationQueryService, ObservationQueryService>();

//builder.Services.AddScoped<IObservationsAnalysisService, ObservationsAnalysisService>();

//builder.Services.AddScoped<IFlickrService, FlickrService>();
//builder.Services.AddScoped<IBirdThumbnailPhotoService, BirdThumbnailPhotoService>();

//builder.Services.AddSingleton<ISystemClockService, SystemClockService>();
//builder.Services.AddSingleton<IUrlService, UrlService>();

//builder.Services.AddHttpClient();
//builder.Services.AddScoped<IXenoCantoService, XenoCantoService>();

//builder.Services.AddTransient<IEmailSender, EmailSender>();
////builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.("")); // Configuration);


//var baseUrl = string.Concat(builder.Configuration.GetValue<string>("Scheme"), builder.Configuration.GetValue<string>("Domain"));

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = baseUrl,
//        ValidAudience = baseUrl,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("TokenKey")))
//    };
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(MyAllowSpecificOrigins,
//    builder =>
//    {
//        builder.WithOrigins(baseUrl);
//    });
//});

//builder.Services.AddAzureClients(clientBuilder =>
//{
//    clientBuilder.AddBlobServiceClient(builder.Configuration.GetValue<string>("ConnectionStrings:StorageConnection:blob"), preferMsi: true);
//    clientBuilder.AddQueueServiceClient(builder.Configuration.GetValue<string>("ConnectionStrings:StorageConnection:queue"), preferMsi: true);
//});

//app.UseStaticFiles();
//app.UseSpaStaticFiles();

//app.UseRouting();

//app.UseCors(MyAllowSpecificOrigins);
//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapDefaultControllerRoute();
//    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
//});

//app.UseSpa(spa =>
//{
//    // To learn more about options for serving an Angular SPA from ASP.NET Core,
//    // see https://go.microsoft.com/fwlink/?linkid=864501
//    spa.Options.SourcePath = "ClientApp";
//    if (env.IsDevelopment())
//    {
//        spa.UseAngularCliServer(npmScript: "start");
//    }
//});


//app.Run();





//internal static class StartupExtensions
//{
//    public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
//    {
//        if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
//        {
//            return builder.AddBlobServiceClient(serviceUri);
//        }
//        else
//        {
//            return builder.AddBlobServiceClient(serviceUriOrConnectionString);
//        }
//    }
//    public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
//    {
//        if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
//        {
//            return builder.AddQueueServiceClient(serviceUri);
//        }
//        else
//        {
//            return builder.AddQueueServiceClient(serviceUriOrConnectionString);
//        }
//    }
//}




// Program.cs generated on new project (11/11/2021
// Angular Spa template with Identity

//using DotNet6.Data;
//using DotNet6.Models;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddIdentityServer()
//    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

//builder.Services.AddAuthentication()
//    .AddIdentityServerJwt();

//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseMigrationsEndPoint();
//}
//else
//{
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();

//app.UseAuthentication();
//app.UseIdentityServer();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action=Index}/{id?}");
//app.MapRazorPages();

//app.MapFallbackToFile("index.html"); ;

//app.Run();