namespace SSMB.Application.Items.Queries.GetBulkAppraisal
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

    class GetBulkUnderCutQueryHandler : IRequestHandler<GetBulkUnderCutQuery, ItemRecommendedPrice[]>
    {
        private readonly ISsmbDbContext dbContext;

        public GetBulkUnderCutQueryHandler(ISsmbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ItemRecommendedPrice[]> Handle(GetBulkUnderCutQuery request, CancellationToken cancellationToken)
        {
            bool allowDirectSell = false;
            var items = new List<ItemRecommendedPrice>();
            foreach (var itemName in request.Items)
            {
                var item = await this.dbContext.Items.Where(i => i.Name.Equals(itemName)).FirstOrDefaultAsync(cancellationToken);
                if (item == null)
                {
                    continue;
                }

                var batch = await this.dbContext
                                      .OrderBatch
                                      .Where(o => o.ItemId == item.ItemId)
                                      .OrderByDescending(o => o.Date)
                                      .Include(o => o.Entries)
                                      .FirstOrDefaultAsync(cancellationToken);

                if (batch == null || !batch.SaleEntries.Any())
                {
                    items.Add(new ItemRecommendedPrice
                    {
                        Item = item,
                        Price = 0,
                        Reason = "No Comparisons Available"
                    });
                    
                    continue;
                }

                long currentMaxPurchasePrice = item.ScrapValue;
                if (batch.PurchaseEntries.Any())
                {
                    currentMaxPurchasePrice = Math.Max(currentMaxPurchasePrice, batch.PurchaseEntries.OrderByDescending(e => e.Price).First().Price);
                }

                var currentMinSale = batch.SaleEntries.OrderBy(e => e.Price).First().Price;
                if (currentMaxPurchasePrice > currentMinSale && allowDirectSell)
                {
                    items.Add(new ItemRecommendedPrice
                    {
                        Item = item,
                        Price = 0,
                        Reason = "Direct Sell"
                    });

                    continue;
                }

                var undercutValue = (long)(currentMinSale * 0.9);
                var value = Math.Max(undercutValue, currentMaxPurchasePrice);
                items.Add(new ItemRecommendedPrice
                {
                    Item = item,
                    Price = value
                });
            }

            return items.ToArray();
        }
    }
}
