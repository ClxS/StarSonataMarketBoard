namespace SSMB.Blazor.ViewServices
{
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Items.Models;
    using Domain;
    using Server.API.V1;

    internal class ItemsServiceServerBased : IItemsService
    {
        private readonly ItemsController itemsController;

        public ItemsServiceServerBased(ItemsController itemsController)
        {
            this.itemsController = itemsController;
        }

        public Task<Item[]> GetItemsMatchingFilter(string filter)
        {
            return Task.Run(async () => (await this.itemsController.Index(filter)).ToArray());
        }

        public Task<RecentItem[]> GetRecentlyUpdatedItems()
        {
            return Task.Run(async () => (await this.itemsController.Recent()).ToArray());
        }

        public Task<Item[]> GetHotItems(int count)
        {
            return Task.Run(async () => (await this.itemsController.Hot()).ToArray());
        }

        public Task<FullDetailItem> GetItemDetails(int id)
        {
            return Task.Run(async () => (await this.itemsController.Detail(id)));
        }
    }
}