namespace SSMB.Blazor.ViewServices
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Application.Items.Models;
    using Domain;
    using Microsoft.AspNetCore.Components;
    using Server.API.V1;

    public interface IItemsService
    {
        Task<Item[]> GetItemsMatchingFilter(string filter);

        Task<RecentItem[]> GetRecentlyUpdatedItems();
    }

    internal abstract class ItemsServiceClientBased : IItemsService
    {
        private readonly IHttpClientFactory httpClientFactory;

        protected ItemsServiceClientBased(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public virtual async Task<Item[]> GetItemsMatchingFilter(string filter)
        {
            var items = await this.httpClientFactory.CreateClient()
                                  .GetJsonAsync<Item[]>($"https://localhost:44327/api/v1/items?filter={filter}");
            return items;
        }

        public virtual async Task<RecentItem[]> GetRecentlyUpdatedItems()
        {
            var items = await this.httpClientFactory.CreateClient()
                                  .GetJsonAsync<RecentItem[]>($"https://localhost:44327/api/v1/items/recent");
            return items;
        }
    }

    internal class ItemsServiceServerBased : IItemsService
    {
        private readonly ItemsController itemsController;

        public ItemsServiceServerBased(ItemsController itemsController)
        {
            this.itemsController = itemsController;
        }

        public Task<Item[]> GetItemsMatchingFilter(string filter)
        {
            return Task.Run(() => this.itemsController.Index(filter).ToArray());
        }

        public Task<RecentItem[]> GetRecentlyUpdatedItems()
        {
            return Task.Run(async () => (await this.itemsController.Recent()).ToArray());
        }
    }
}
