namespace SSMB.Application.Items.Queries.GetBulkAppraisal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Models;

    class GetBulkAppraisalQueryHandler : IRequestHandler<GetBulkAppraisalQuery, ItemAppraisal[]>
    {
        private readonly ISsmbDbContext dbContext;

        public GetBulkAppraisalQueryHandler(ISsmbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ItemAppraisal[]> Handle(GetBulkAppraisalQuery request, CancellationToken cancellationToken)
        {
            var items = new List<ItemAppraisal>();
            foreach (var (itemName, itemCount) in request.Items)
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
                if (batch == null || batch.PurchaseEntries.All(e => e.BaseName.Contains("ClxS")))
                {
                    if (item.ScrapValue > 0)
                    {
                        items.Add(new ItemAppraisal
                        {
                            Item = item,
                            DateUpdated = DateTime.Now,
                            TotalProfit = itemCount * item.ScrapValue,
                            Sales = new (ItemAppraisal.SellType Type, int Count, string To)[] { (ItemAppraisal.SellType.Scrap, itemCount, null) }
                        });

                        continue;
                    }
                    else
                    {
                        items.Add(new ItemAppraisal
                        {
                            Item = item,
                            DateUpdated = DateTime.Now,
                            TotalProfit = 0,
                        });
                        continue;
                    }
                }

                var sales = new List<(ItemAppraisal.SellType Type, int Count, string To)>();
                var totalProfit = 0L;
                var count = itemCount;
                var selling = new Queue<(long price, string @base, int amount)>(
                    batch.PurchaseEntries
                         .Where(b => !b.BaseName.Contains("ClxS"))
                         .OrderByDescending(e => e.Price)
                        .Select(e => (e.Price, $"{e.BaseName} - {e.GalaxyName}", e.Quantity)));
                var currentSell = selling.Dequeue();
                while (true)
                {
                    if (currentSell.amount <= 0)
                    {
                        if (selling.Count <= 0)
                        {
                            break;
                        }

                        currentSell = selling.Dequeue();
                    }

                    if (count <= 0)
                    {
                        break;
                    }

                    if (currentSell.price <= item.ScrapValue)
                    {
                        sales.Add((ItemAppraisal.SellType.Scrap, count, null));
                        totalProfit += item.ScrapValue * count;
                        break;
                    }
                    
                    var amountToSell = Math.Min(count, currentSell.amount);
                    count -= amountToSell;
                    currentSell = (currentSell.price, currentSell.@base, currentSell.amount - amountToSell);

                    totalProfit += currentSell.price * amountToSell;
                    sales.Add((ItemAppraisal.SellType.ToBase, amountToSell, currentSell.@base));
                }

                items.Add(new ItemAppraisal
                {
                    Item = item,
                    TotalProfit = totalProfit,
                    DateUpdated = batch.Date,
                    Sales = sales.ToArray()
                });
            }

            return items.OrderByDescending(i => i?.TotalProfit ?? 0).ToArray();
        }
    }
}
