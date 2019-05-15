namespace SSMB.DataCollection.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Hangfire;
    using Items;
    using Newtonsoft.Json;

    class FindItemsToCheckExist
    {
        private readonly Func<ISsmbDbContext> dbContextFactory;
        private readonly IEnumerable<IItemProvider> itemProviders;
        private readonly IMarketCheckService marketCheckService;

        public FindItemsToCheckExist(IEnumerable<IItemProvider> itemProviders, IMarketCheckService marketCheckService,
            Func<ISsmbDbContext> dbContextFactory)
        {
            this.marketCheckService = marketCheckService;
            this.itemProviders = itemProviders;
            this.dbContextFactory = dbContextFactory;
        }

        [MaximumConcurrentExecutions(1)]
        [Queue("gather_checkable")]
        public async Task DoFindItemsToCheckExist()
        {
            return;
            Console.WriteLine("Starting Job: FindItemsToCheckExist");

            using var dbContext = this.dbContextFactory();
            foreach (var provider in this.itemProviders)
            {
                foreach (var item in provider.GetItems())
                {
                    if (!dbContext.Items.Any(i => i.Name == item.name))
                    {
                        var valueStr = JsonConvert.SerializeObject(item.values);
                        BackgroundJob.Enqueue<CheckItemExistsJob>(job => job.CheckItemExists(
                            item.name,
                            item.type,
                            item.cost,
                            item.weight,
                            item.space,
                            item.quality,
                            valueStr));
                    }
                }
            }
        }
    }
}
