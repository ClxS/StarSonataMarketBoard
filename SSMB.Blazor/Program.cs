namespace SSMB.Blazor
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseUrls("http://localhost:5050");
                    webBuilder.ConfigureLogging((context, logging) =>
                    {
                        logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
                    });
                    webBuilder.ConfigureAppConfiguration((context, config) =>
                    {
                        var env = context.HostingEnvironment;
                        config.AddJsonFile("appsettings.json", true, true)
                              .AddJsonFile($"appsettings.Secrets.json", true, true)
                              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                        config.AddEnvironmentVariables();
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}
