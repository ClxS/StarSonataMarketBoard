namespace SSMB.DataCollection.DataCollection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces;
    using Autofac;
    using Hangfire;
    using Jobs;
    using Microsoft.EntityFrameworkCore;
    using SQL;
    using Utility;

    internal class DataCollectionService : IDataCollectionService, IStartable
    {
        private readonly Func<ISsmbDbContext> dbContextFactory;

        public DataCollectionService(Func<ISsmbDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public void Start()
        {
            Console.WriteLine("Developer: SQL Erasing Hangfire");
            Console.WriteLine("Preparing Hangfire");
            Console.ForegroundColor = ConsoleColor.White;

            using (var dbContext = this.dbContextFactory())
            {
                // If it's a known context type, lets take a shortcut and clear the table directly.
                /*if (dbContext is SsmbDbContext concreteContext)
                {
                    concreteContext.Database.ExecuteSqlCommand("DELETE FROM HangFire.Job");
                    concreteContext.Database.ExecuteSqlCommand("DELETE FROM HangFire.JobQueue");
                }*/

                foreach (var name in dbContext.Items.Select(i => i.Name))
                {
                    RecurringJob.AddOrUpdate<ItemMarketCheck>(ItemMarketCheck.GetJobName(name),
                        (job) => job.DoItemMarketCheck(name), CronUtility.GetRandom12HourCron(), null, "update_mc");
                }
            }

            /*this.EraseQueue("default");
            this.EraseQueue("check_available");
            this.EraseQueue("gather_checkable");
            this.EraseQueue("update_mc");*/

            //RecurringJob.RemoveIfExists("FindItemsToCheckExist");
            RecurringJob.AddOrUpdate<FindItemsToCheckExist>("FindItemsToCheckExist", (job) => job.DoFindItemsToCheckExist(), Cron.Monthly, null, "gather_checkable");
            //RecurringJob.Trigger("FindItemsToCheckExist");
        }

        private void EraseQueue(string queueName)
        {
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            var queues = monitoringApi.Queues();

            var checkableQueue = queues.FirstOrDefault(q => q.Name == queueName);
            if (checkableQueue != null)
            {
                Console.WriteLine($"\nErasing Queue {queueName} - Count: {checkableQueue.Length}");
                var originalCount = (int)checkableQueue.Length;
                var count = originalCount;
                while (count > 0)
                {
                    int progress = (int)(20.0 * ((double)count / originalCount));
                    Console.Write($"\r[{new string('=', 20 - progress)}{new string(' ', progress)}] ");

                    var toDelete = new List<string>();
                    var iterationCount = Math.Min(200, count);
                    count -= iterationCount;

                    monitoringApi.EnqueuedJobs(queueName, 0, iterationCount)
                                 .ForEach(x => toDelete.Add(x.Key));
                    foreach (var jobId in toDelete)
                    {
                        BackgroundJob.Delete(jobId);
                    }
                }
            }
        }
    }
}
