using AutoMapper.EquivalencyExpression;
using Azure.Core.Extensions;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Birder
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // In production, the Angular files will be served from this directory
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/dist";
            //});

            services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
                // Password settings: require any eight letters or numbers
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

            services.AddAutoMapper((serviceProvider, automapper) =>
            {
                automapper.AddCollectionMappers();
                //automapper.UseEntityFrameworkCoreModel<ApplicationDbContext>(serviceProvider);
            }, typeof(ApplicationDbContext).Assembly);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBirdRepository, BirdRepository>();
            services.AddScoped<IObservationRepository, ObservationRepository>();
            services.AddScoped<IObservationPositionRepository, ObservationPositionRepository>();
            services.AddScoped<IObservationNoteRepository, ObservationNoteRepository>();
            services.AddScoped<INetworkRepository, NetworkRepository>();
            services.AddScoped<ITweetDayRepository, TweetDayRepository>();
            
            services.AddScoped<IServerlessDatabaseService, ServerlessDatabaseService>();
            services.AddScoped<IListService, ListService>();
            services.AddScoped<IObservationQueryService, ObservationQueryService>();

            services.AddScoped<IObservationsAnalysisService, ObservationsAnalysisService>();

            services.AddScoped<IFlickrService, FlickrService>();
            services.AddScoped<IBirdThumbnailPhotoService, BirdThumbnailPhotoService>();

            services.AddScoped<IAuthenticationTokenService, AuthenticationTokenService>();

            services.AddSingleton<ISystemClockService, SystemClockService>();
            services.AddSingleton<IUrlService, UrlService>();

            services.AddHttpClient();
            services.AddScoped<IXenoCantoService, XenoCantoService>();

            // CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            // CloudConfigurationManager.GetSetting("StorageConnectionString"));

            //services.AddScoped<IFileClient, AzureBlobFileClient>(client =>
            //{
            //    var cloudConnStr = Configuration["StorageConnectionString"];
            //    return new AzureBlobFileClient(cloudConnStr);
            //});

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            var baseUrl = string.Concat(Configuration["Scheme"], Configuration["Domain"]);

            services.AddAuthentication(options =>
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
                    ValidIssuer = baseUrl,
                    ValidAudience = baseUrl,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]))
                };
            });

            services.AddCors(options =>
            {
               options.AddPolicy(MyAllowSpecificOrigins,
               builder =>
               {
                   builder.WithOrigins(baseUrl);
               });
            });
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(Configuration["ConnectionStrings:StorageConnection:blob"], preferMsi: true);
                builder.AddQueueServiceClient(Configuration["ConnectionStrings:StorageConnection:queue"], preferMsi: true);
            });
        }

        // This method gets called by the runtime. 
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            //app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

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
        }
    }
    internal static class StartupExtensions
    {
        public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
            {
                return builder.AddBlobServiceClient(serviceUri);
            }
            else
            {
                return builder.AddBlobServiceClient(serviceUriOrConnectionString);
            }
        }
        public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
            {
                return builder.AddQueueServiceClient(serviceUri);
            }
            else
            {
                return builder.AddQueueServiceClient(serviceUriOrConnectionString);
            }
        }
    }
}
