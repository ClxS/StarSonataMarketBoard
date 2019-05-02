namespace SSMB.Blazor.Shared.RecentItems
{
    using System;
    using Application.Items.Models;
    using Domain;

    public interface IRecentItemsViewModel
    {
        RecentItem[] Results { get; }

        IObservable<RecentItem[]> WhenResultsChanged { get; }
    }
}
