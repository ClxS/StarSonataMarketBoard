namespace SSMB.Application.Items.Queries.GetFullItemDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Models;

    class GetFullItemDetailsQueryHandler : IRequestHandler<GetFullItemDetailsQuery, FullDetailItem>
    {
        private readonly ISsmbDbContext dbContext;

        public GetFullItemDetailsQueryHandler(ISsmbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<FullDetailItem> Handle(GetFullItemDetailsQuery request, CancellationToken cancellationToken)
        {
            var item = await this.dbContext.Items
                            .FirstAsync(i => i.ItemId == request.ItemId, cancellationToken);
            var orders = await this.dbContext.OrderBatch
                                   .OrderByDescending(o => o.Date)
                                   .Where(o => o.ItemId == request.ItemId)
                                   .Take(50)
                                   .Include(o => o.Entries)
                                   .Include(o => o.LtsEntries)
                                   .ToArrayAsync(cancellationToken);
            var lastOrder = orders.FirstOrDefault();
            orders = orders.Reverse().ToArray();

            var purchasePrices = new List<(DateTime date, long price, long quantity)>();
            var salePrices = new List<(DateTime date, long price, long quantity)>();
            foreach (var order in orders)
            {
                if (order.IsLtsOrder)
                {
                    var entries = order.LtsEntries.Cast<IOrderEntry>().ToArray();
                    purchasePrices.Add(this.GetPriceFromEntry(entries, OrderType.Purchase, order.Date));
                    salePrices.Add(this.GetPriceFromEntry(entries, OrderType.Selling, order.Date));
                }
                else
                {
                    var entries = order.Entries.Cast<IOrderEntry>().ToArray();
                    purchasePrices.Add(this.GetPriceFromEntry(entries, OrderType.Purchase, order.Date));
                    salePrices.Add(this.GetPriceFromEntry(entries, OrderType.Selling, order.Date));
                }
            }

            item.StructuredDescription = string.Empty;
            return new FullDetailItem
            {
                Item = item,
                LatestOrder = lastOrder,
                PurchasePrices = purchasePrices.OrderBy(p => p.price).ToList(),
                SalePrices = salePrices.OrderByDescending(p => p.price).ToList()
            };
        }

        private (DateTime date, long price, long quantity) GetPriceFromEntry(IOrderEntry[] orders, OrderType type, DateTime date)
        {
            var lastFoundValue = type == OrderType.Purchase ? 0L : long.MaxValue;
            var lastFoundQuantity = -1L;
            if (orders.All(p => p.OrderType != type))
            {
                return (date, lastFoundValue, lastFoundQuantity);
            }

            foreach (var order in orders)
            {
                if (order.OrderType != type)
                {
                    continue;
                }

                if ((type != OrderType.Purchase || lastFoundValue >= order.Price) &&
                    (type != OrderType.Selling || lastFoundValue <= order.Price))
                {
                    continue;
                }

                lastFoundValue = order.Price;
                lastFoundQuantity = order.Quantity;
            }

            return (date, lastFoundValue, lastFoundQuantity);
        }
    }
}
