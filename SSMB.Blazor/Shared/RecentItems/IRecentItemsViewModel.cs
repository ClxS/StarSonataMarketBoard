namespace SSMB.Blazor.Shared.RecentItems
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Application.Items.Models;
    using Domain;
    using ViewServices;

    public interface IRecentItemsViewModel
    {
        RecentItem[] Results { get; }

        IObservable<RecentItem[]> WhenResultsChanged { get; }
    }

    class RecentItemsViewModel : IRecentItemsViewModel
    {
        private readonly Subject<RecentItem[]> resultsChangedSubject;
        private RecentItem[] results;

        public RecentItemsViewModel(IItemsService itemsService)
        {
            this.resultsChangedSubject = new Subject<RecentItem[]>();
            itemsService.GetRecentlyUpdatedItems().ContinueWith(items => { this.Results = items.Result; });
        }

        public RecentItem[] Results
        {
            get => this.results;
            set
            {
                this.results = value;
                this.resultsChangedSubject.OnNext(value);
            }
        }

        public IObservable<RecentItem[]> WhenResultsChanged => this.resultsChangedSubject.AsObservable();
    }
}
