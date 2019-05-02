namespace SSMB.Blazor.Services
{
    using System;
    using System.Reactive;
    using Domain;

    public interface ISearchService
    {
        Item[] SearchResults { get; }
        string SearchValue { get; set; }

        IObservable<string> WhenSearchChanged { get; }

        IObservable<Unit> WhenSearchMarkedVisible { get; }

        IObservable<Item[]> WhenSearchResultsChanged { get; }

        void MarkSearchVisible();
    }
}
