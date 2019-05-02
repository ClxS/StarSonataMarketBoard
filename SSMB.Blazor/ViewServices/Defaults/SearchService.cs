namespace SSMB.Blazor.ViewServices.Defaults
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Domain;
    using Services;

    internal class SearchService : ISearchService
    {
        private readonly IItemsService itemsService;

        private readonly Subject<string> searchChangedSubject = new Subject<string>();

        private readonly Subject<Unit> searchMarkedVisibleSubject = new Subject<Unit>();

        private readonly Subject<Item[]> searchResultsSubject;

        private Item[] searchResults;

        private string searchValue;

        public SearchService(IItemsService itemsService)
        {
            this.itemsService = itemsService;
            this.searchResultsSubject = new Subject<Item[]>();
            this.WhenSearchChanged.Throttle(TimeSpan.FromMilliseconds(100)).Subscribe(v =>
            {
                if (string.IsNullOrEmpty(v))
                {
                    this.SearchResults = null;
                }
                else if (v.Length >= 3)
                {
                    this.Search(v);
                }
            });
        }

        public Item[] SearchResults
        {
            get => this.searchResults;
            private set
            {
                this.searchResults = value;
                this.searchResultsSubject.OnNext(value);
            }
        }

        public string SearchValue
        {
            get => this.searchValue;
            set
            {
                if (value.Equals(this.searchValue))
                {
                    return;
                }

                this.searchValue = value;
                this.searchChangedSubject.OnNext(value);
            }
        }

        public IObservable<string> WhenSearchChanged => this.searchChangedSubject.AsObservable();

        public IObservable<Unit> WhenSearchMarkedVisible => this.searchMarkedVisibleSubject.AsObservable();

        public IObservable<Item[]> WhenSearchResultsChanged => this.searchResultsSubject.AsObservable();

        public void MarkSearchVisible()
        {
            this.searchMarkedVisibleSubject.OnNext(Unit.Default);
        }

        private void Search(string s)
        {
            this.itemsService.GetItemsMatchingFilter(s).ContinueWith(items => { this.SearchResults = items.Result; });
        }
    }
}
