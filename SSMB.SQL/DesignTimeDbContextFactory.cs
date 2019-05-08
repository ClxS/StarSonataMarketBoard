namespace SSMB.SQL
{
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SsmbDbContext>
    {
        public SsmbDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SsmbDbContext>();
            var connectionString = "Data Source=localhost\\SQLEXPRESS01;Initial Catalog=ssmb;Integrated Security=True";
            builder.UseSqlServer(connectionString);
            return new SsmbDbContext(builder.Options);
        }
    }
}
