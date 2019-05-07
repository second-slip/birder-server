using AutoMapper;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions
                (options => {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                //options.SignIn.RequireConfirmedEmail = true;
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
                .AddSignInManager<SignInManager<ApplicationUser>>();
            //.AddDefaultTokenProviders();


            //services.AddAutoMapper();
            services.AddAutoMapper(GetType().Assembly);

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<BirderMappingProfile>();
            });

            //services.AddTransient<IRepository, Repository>();
            //services.AddSignInManager<SignInManager<IdentityUser>>();
            services.AddTransient<IBirdRepository, BirdRepository>();
            services.AddTransient<IObservationRepository, ObservationRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddScoped<IObservationsAnalysisRepository, ObservationsAnalysisRepository>();
            //services.AddTransient<ITweetDayRepository, TweetDayRepository>();

            services.AddSingleton<ISystemClock, SystemClock>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
            
                    ValidIssuer = "http://localhost:55722",
                    ValidAudience = "http://localhost:55722",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();

            app.UseCors("EnableCORS");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
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
