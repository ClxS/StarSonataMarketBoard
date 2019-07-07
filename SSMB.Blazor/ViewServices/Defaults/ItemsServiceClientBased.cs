namespace SSMB.Blazor.ViewServices
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Application.Items.Models;
    using Domain;
    using Microsoft.AspNetCore.Components;

    internal abstract class ItemsServiceClientBased : IItemsService
    {
        private readonly IHttpClientFactory httpClientFactory;

        protected ItemsServiceClientBased(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Item[]> GetItemsMatchingFilter(string filter)
        {
            var items = await this.httpClientFactory.CreateClient()
                                  .GetJsonAsync<Item[]>($"https://localhost:44327/api/v1/items?filter={filter}");
            return items;
        }

        public async Task<RecentItem[]> GetRecentlyUpdatedItems()
        {
            var items = await this.httpClientFactory.CreateClient()
                                  .GetJsonAsync<RecentItem[]>($"https://localhost:44327/api/v1/items/recent");
            return items;
        }

        public async Task<Item[]> GetHotItems(int count)
        {
            var items = await this.httpClientFactory.CreateClient()
                                  .GetJsonAsync<Item[]>($"https://localhost:44327/api/v1/items/hot?count={count}");
            return items;
        }

        public async Task<FullDetailItem> GetItemDetails(int id)
        {
            var items = await this.httpClientFactory.CreateClient()
                                  .GetJsonAsync<FullDetailItem>($"https://localhost:44327/api/v1/items/detail?id={id}");
            return items;
        }

        public async Task<ItemProfit[]> GetProfitableItems()
        {
            var items = await this.httpClientFactory.CreateClient()
                                  .GetJsonAsync<ItemProfit[]>("https://localhost:44327/api/v1/items/profitable");
            return items;
        }

        public Task<ItemAppraisal[]> GetAppraisal((string Name, int Count)[] itemNames)
        {
            throw new NotImplementedException();
        }

        public Task<ItemRecommendedPrice[]> GetUnderCut(string[] itemNames)
        {
            throw new NotImplementedException();
        }
    }
}