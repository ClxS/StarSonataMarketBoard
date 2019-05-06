namespace SSMB.Blazor.Shared.HottestItems
{
    using System;
    using Domain;

    public interface IHottestItemsViewModel
    {
        Item[] Items { get; }

        IObservable<Item[]> WhenItemsChanged { get; }
    }
}
