namespace SSMB.Application.Items.Queries.GetRecentItems
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Models;

    class GetRecentItemsQueryHandler : IRequestHandler<GetRecentItemsQuery, RecentItem[]>
    {
        private readonly ISsmbDbContext dbContext;

        public GetRecentItemsQueryHandler(ISsmbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<RecentItem[]> Handle(GetRecentItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await this.dbContext.Items
                                  .Where(i => i.Orders.Any())
                                  .OrderByDescending(i => i.Orders.Max(o => o.Date))
                                  .Take(20)
                                  .ToArrayAsync(cancellationToken);
            var recentItems = items.Select(i => new RecentItem { Item = i }).ToArray();
            foreach (var item in recentItems)
            {
                var latestOrder = await this.dbContext.OrderBatch.Where(o => o.ItemId == item.Item.ItemId).OrderByDescending(o => o.Date).Include(o => o.Entries)
                                            .FirstAsync(cancellationToken);
                item.DateChecked = latestOrder.Date;
                if (latestOrder.Entries != null && latestOrder.Entries.Any(e => e.OrderType == Domain.OrderType.Purchase))
                {
                    var purchases = latestOrder.Entries.Where(e => e.OrderType == Domain.OrderType.Purchase).ToArray();
                    item.PurchaseQuantity = purchases.Any(s => s.Quantity == int.MaxValue) ? (long?)null : purchases.Sum(e => e.Quantity);
                    item.LastHighestPurchasePrice = latestOrder
                                                    .Entries.Where(e => e.OrderType == Domain.OrderType.Purchase)
                                                    .Max(o => o.Price);
                }

                if (latestOrder.Entries != null && latestOrder.Entries.Any(e => e.OrderType == Domain.OrderType.Selling))
                {
                    var sales = latestOrder.Entries.Where(e => e.OrderType == Domain.OrderType.Selling).ToArray();
                    item.SaleQuantity = sales.Any(s => s.Quantity == int.MaxValue) ? (long?)null : sales.Sum(e => e.Quantity);
                    item.LastLowestSalePrice = latestOrder
                                               .Entries.Where(e => e.OrderType == Domain.OrderType.Selling)
                                               .Min(o => o.Price);
                }
            }

            return recentItems;
        }
    }
}
