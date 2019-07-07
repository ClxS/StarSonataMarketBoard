namespace SSMB.Blazor.Pages.Index.ProfitableItems
{
    using System;
    using Application.Items.Models;

    public interface IProfitableItemsViewModel
    {
        ItemProfit[] Results { get; }

        IObservable<ItemProfit[]> WhenResultsChanged { get; }

        void OnItemClicked(int itemItemId);
    }
}
