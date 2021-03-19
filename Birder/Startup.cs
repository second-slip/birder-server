using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

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
            //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // var t = @"Server=tcp:birder.database.windows.net,1433;Initial Catalog=BirderDb;Persist Security Info=False;User ID=X;Password=Y;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False; Connection Timeout=30;";
            //var config = new StringBuilder(Configuration["ConnectionStrings: DefaultConnection"]);
            //string conn = config.Replace("ENVPW", Configuration["DB_PW"])
            //                    .ToString();
            services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            // options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn));

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
            services.AddScoped<IListService, ListService>();

            services.AddScoped<IObservationsAnalysisService, ObservationsAnalysisService>();

            services.AddScoped<IFlickrService, FlickrService>();
            services.AddScoped<IBirdThumbnailPhotoService, BirdThumbnailPhotoService>();

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
        }

        // This method gets called by the runtime. 
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
