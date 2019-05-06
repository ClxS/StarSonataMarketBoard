namespace SSMB.Blazor.Pages.Item.LatestHistory
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using Application.Items.Models;
    using Domain;

    public interface ILatestHistoryViewModel
    {
        FullDetailItem ItemDetails { get; }

        IEnumerable<OrderEntry> OrderedPurchaseEntries { get; }

        OrderType OrderType { get; }

        IObservable<Unit> WhenOrderedEntriesUpdated { get; }

        void SetDetails(OrderType type, FullDetailItem itemType);
    }
}
