namespace SSMB.Blazor.Pages.Index.RecentItems
{
    using System;
    using Application.Items.Models;

    public interface IRecentItemsViewModel
    {
        RecentItem[] Results { get; }

        IObservable<RecentItem[]> WhenResultsChanged { get; }

        void OnItemClicked(int itemItemId);
    }
}
