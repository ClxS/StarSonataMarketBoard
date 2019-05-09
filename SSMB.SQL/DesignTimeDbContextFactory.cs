namespace SSMB.SQL
{
    using System;
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SsmbDbContext>
    {
        public SsmbDbContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Build config
            IConfiguration config = new ConfigurationBuilder()
                                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SSMB.Blazor"))
                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                                    .Build();
            var builder = new DbContextOptionsBuilder<SsmbDbContext>();
            var connectionString = config.GetConnectionString("SSMBDatabase");

            Console.WriteLine($"ConnectionString: {connectionString}");
            builder.UseSqlServer(connectionString);
            return new SsmbDbContext(builder.Options);
        }
    }
}
