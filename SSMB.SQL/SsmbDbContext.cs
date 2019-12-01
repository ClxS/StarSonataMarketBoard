namespace SSMB.SQL
{
    using System;
    using Application.Interfaces;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    public sealed class SsmbDbContext : DbContext, ISsmbDbContext
    {
        private static bool created = false;

        public SsmbDbContext(DbContextOptions options)
            : base(options)
        {
            if (!created)
            {
                created = true;
                this.Database.EnsureCreated();

                try
                {
                    this.Database.Migrate();
                }
                catch
                {
                    Console.WriteLine("Encountered error trying to migrate");
                }
            }
        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Alert> Alerts { get; set; }

        public DbSet<AlertCondition> AlertConditions { get; set; }

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
