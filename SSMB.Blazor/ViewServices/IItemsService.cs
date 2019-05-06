namespace SSMB.Blazor.ViewServices
{
    using System.Threading.Tasks;
    using Application.Items.Models;
    using Domain;

    public interface IItemsService
    {
        Task<Item[]> GetItemsMatchingFilter(string filter);

        Task<RecentItem[]> GetRecentlyUpdatedItems();

        Task<Item[]> GetHotItems(int count);

        Task<FullDetailItem> GetItemDetails(int id);
    }
}
