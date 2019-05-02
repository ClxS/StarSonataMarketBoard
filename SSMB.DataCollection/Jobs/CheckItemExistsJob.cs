namespace SSMB.DataCollection.Jobs
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Domain;
    using Hangfire;
    using Utility;

    class CheckItemExistsJob
    {
        private readonly Func<ISsmbDbContext> dbContextFactory;
        private readonly IMarketCheckService marketCheckService;

        public CheckItemExistsJob(IMarketCheckService marketCheckService, Func<ISsmbDbContext> dbContextFactory)
        {
            this.marketCheckService = marketCheckService;
            this.dbContextFactory = dbContextFactory;
        }

        [MaximumConcurrentExecutions(1)]
        [Queue("check_available")]
        public async Task CheckItemExists(string name, ItemType type, long cost, long weight, long space,
            Quality quality, string structuredDescription)
        {
            var result = await this.marketCheckService.RequestMarketCheckWithDescription(name);
            if (result.orders != null && result.orders.Length > 0)
            {
                using (var dbContext = this.dbContextFactory())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Added Item: {name}");
                    Console.ForegroundColor = ConsoleColor.White;
                    RecurringJob.AddOrUpdate<ItemMarketCheck>(ItemMarketCheck.GetJobName(name),
                        (job) => job.DoItemMarketCheck(name), CronUtility.GetRandom12HourCron(), null, "update_mc");

                    var item = new Item()
                    {
                        Name = name,
                        Description = result.description,
                        StructuredDescription = structuredDescription,
                        Cost = cost,
                        Type = type,
                        Weight = weight,
                        Space = (int)space,
                        ScrapValue = result.scrapValue,
                        Quality = quality
                    };

                    dbContext.AddOrUpdateItem(item);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
