namespace SSMB.Application.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public interface ISsmbDbContext : IDisposable
    {
        DbSet<Item> Items { get; set; }

        DbSet<Alert> Alerts { get; set; }

        DbSet<LtsOrderEntry> LtsOrder { get; set; }

        DbSet<OrderBatch> OrderBatch { get; set; }

        DbSet<OrderEntry> OrderEntry { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
