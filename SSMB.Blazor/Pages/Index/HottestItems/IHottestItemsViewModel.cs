namespace SSMB.Blazor.Pages.Index.HottestItems
{
    using System;
    using Domain;

    public interface IHottestItemsViewModel
    {
        Item[] Items { get; }

        IObservable<Item[]> WhenItemsChanged { get; }

        void OnItemClicked(int itemItemId);
    }
}
