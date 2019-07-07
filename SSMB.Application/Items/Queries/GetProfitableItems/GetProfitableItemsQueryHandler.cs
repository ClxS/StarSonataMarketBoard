namespace SSMB.Application.Items.Queries.GetProfitableItems
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Models;

    class GetProfitableItemsQueryHandler : IRequestHandler<GetProfitableItemsQuery, ItemProfit[]>
    {
        private readonly ISsmbDbContext dbContext;

        public GetProfitableItemsQueryHandler(ISsmbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ItemProfit[]> Handle(GetProfitableItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await this.dbContext.Items
                                  .Where(i => i.Orders.Any())
                                  .OrderByDescending(i => i.Orders.Max(o => o.Date))
                                  .ToArrayAsync(cancellationToken);
            Array.ForEach(items, i => i.StructuredDescription = string.Empty);
            var profits = new ConcurrentBag<ItemProfit>();
            foreach(var item in items)
            {
                var batch = await this.dbContext.OrderBatch.Where(o => o.ItemId == item.ItemId)
                                      .OrderByDescending(o => o.Date)
                                      .Include(o => o.Entries).FirstAsync(cancellationToken);
                if (!batch.PurchaseEntries.Any() || !batch.SaleEntries.Any())
                {
                    continue;
                }

                var currentMaxPurchase = batch.PurchaseEntries.OrderByDescending(e => e.Price).First();
                var currentMinSale = batch.SaleEntries.OrderBy(e => e.Price).First();
                var scrapValue = item.ScrapValue;
                if (item.Name == "Solar Intake")
                {
                }

                if (currentMaxPurchase.Price > currentMinSale.Price && currentMaxPurchase.Price > scrapValue)
                {
                    long totalPotentialProfit = currentMaxPurchase.Price - currentMinSale.Price;
                    if (currentMinSale.Quantity < 2000000)
                    {
                        var selling = new Queue<(long price, int amount)>(batch.PurchaseEntries.OrderByDescending(e => e.Price).Select(e => (e.Price, e.Quantity)));
                        var purchasing = new Queue<(long price, int amount)>(batch.SaleEntries.OrderBy(e => e.Price).Select(e => (e.Price, e.Quantity)));

                        var currentSell = selling.Dequeue();
                        var currentPurchase = purchasing.Dequeue();
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

                            if (currentPurchase.amount <= 0)
                            {
                                if (purchasing.Count <= 0)
                                {
                                    break;
                                }

                                currentPurchase = purchasing.Dequeue();
                            }

                            if (currentSell.amount <= 0 || currentPurchase.amount == 0)
                            {
                                break;
                            }

                            if (currentPurchase.price >= currentSell.price)
                            {
                                break;
                            }

                            var amountToSell = Math.Min(currentPurchase.amount, currentSell.amount);
                            totalPotentialProfit += (currentSell.price - currentPurchase.price) * amountToSell;

                            currentSell = (currentSell.price, currentSell.amount - amountToSell);
                            currentPurchase = (currentPurchase.price, currentPurchase.amount - amountToSell);
                        }
                    }

                    profits.Add(new ItemProfit
                    {
                        Item = item,
                        LastChecked = batch.Date,
                        FullPotentialProfit = totalPotentialProfit,
                        PriceDifferential = currentMaxPurchase.Price - currentMinSale.Price,
                        PurchaseStation = currentMaxPurchase.BaseName + " - " + currentMaxPurchase.GalaxyName,
                        SellStation = currentMinSale.BaseName + " - " + currentMinSale.GalaxyName,
                        ShouldScrap = false
                    });
                }
                else if (currentMinSale.Price < scrapValue)
                {
                    var totalPotentialProfit = batch.SaleEntries
                                                    .Where(sale => sale.Price < scrapValue)
                                                    .Sum(sale => (sale.Price - scrapValue) * sale.Quantity);

                    profits.Add(new ItemProfit
                    {
                        Item = item,
                        LastChecked = batch.Date,
                        PriceDifferential = scrapValue - currentMinSale.Price,
                        FullPotentialProfit = totalPotentialProfit,
                        PurchaseStation = currentMaxPurchase.BaseName + " - " + currentMaxPurchase.GalaxyName,
                        SellStation = "-- Scrap --",
                        ShouldScrap = true
                    });
                }
            }

            return profits.OrderByDescending(p => p.FullPotentialProfit).ToArray();
        }
    }
}
