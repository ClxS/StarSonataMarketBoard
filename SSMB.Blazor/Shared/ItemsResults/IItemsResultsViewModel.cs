namespace SSMB.Blazor.Shared.ItemsResults
{
    using System;
    using Domain;

    public interface IItemsResultsViewModel
    {
        bool IsVisible { get; }

        Item[] Results { get; }

        IObservable<Item[]> WhenResultsChanged { get; }

        IObservable<bool> WhenVisibilityChanged { get; }

        void OnWrapperClick();
    }
}
