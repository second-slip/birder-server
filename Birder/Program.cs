using Birder.Data;
using Birder.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Birder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run(); //.MigrateDatabase<ApplicationDbContext>().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(ConfigLogging)
                .UseStartup<Startup>();

        static void ConfigLogging(ILoggingBuilder bldr)
        {
            bldr.AddFilter(DbLoggerCategory.Database.Connection.Name, LogLevel.Information);
        }
    }
}
