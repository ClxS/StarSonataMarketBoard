namespace SSMB.DataCollection.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Domain;
    using Hangfire;

    class ItemMarketCheck
    {
        private readonly Func<ISsmbDbContext> dbContextFactory;
        private readonly IMarketCheckService marketCheckService;

        public ItemMarketCheck(IMarketCheckService marketCheckService, Func<ISsmbDbContext> dbContextFactory)
        {
            this.marketCheckService = marketCheckService;
            this.dbContextFactory = dbContextFactory;
        }

        public static string GetJobName(string item)
        {
            return $"ItemMarketCheck_{item}";
        }

        public async Task DoItemMarketCheck(string name)
        {
            using var dbContext = this.dbContextFactory();
            var item = dbContext.Items.FirstOrDefault(i => i.Name == name);
            if (item == null)
            {
                RecurringJob.RemoveIfExists(GetJobName(name));
                throw new KeyNotFoundException($"Could not find item {name} in database. Task has been removed.");
            }

            var result = await this.marketCheckService.RequestMarketCheck(name);
            if (result != null && result.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Updated Prices on item: {name}");
                Debug.WriteLine($"Updated Prices on item: {name}");
                Console.ForegroundColor = ConsoleColor.White;

                var batch = new OrderBatch()
                {
                    Date = DateTime.Now,
                    Item = item,
                    ItemId = item.ItemId,
                    Entries = result
                };

                dbContext.OrderBatch.Add(batch);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
