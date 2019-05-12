namespace SSMB.SQL
{
    using Application.Interfaces;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    public class SsmbDbContext : DbContext, ISsmbDbContext
    {
        public SsmbDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }

        public DbSet<LtsOrderEntry> LtsOrder { get; set; }

        public DbSet<OrderBatch> OrderBatch { get; set; }

        public DbSet<OrderEntry> OrderEntry { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.QueryClientEvaluationWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SsmbDbContext).Assembly);
        }
    }
}
