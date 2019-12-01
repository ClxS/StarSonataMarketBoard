namespace SSMB.Blazor.Shared.ItemsResults
{
    using System;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Domain;
    using Microsoft.AspNetCore.Components;
    using Services;
    using ViewServices;

    class ItemsResultsViewModel : IItemsResultsViewModel
    {
        private readonly INavigationService navigationService;
        private readonly NavigationManager uriHelper;
        private readonly ISearchService searchService;

        private readonly Subject<bool> visibilityChangeSubject;

        private bool isVisible = true;

        public ItemsResultsViewModel(ISearchService searchService, INavigationService navigationService, NavigationManager uriHelper)
        {
            this.searchService = searchService;
            this.navigationService = navigationService;
            this.uriHelper = uriHelper;
            this.visibilityChangeSubject = new Subject<bool>();
            searchService.WhenSearchMarkedVisible.Subscribe(_ => { this.IsVisible = true; });
            navigationService.WhenBubblePreventingElementClicked.Subscribe(o =>
            {
                if (o.Equals(this))
                {
                    return;
                }

                this.IsVisible = false;
            });
        }

        public bool IsVisible
        {
            get => this.isVisible;
            set
            {
                if (this.isVisible.Equals(value))
                {
                    return;
                }

                Debug.WriteLine($"Is: {value}");
                this.isVisible = value;
                this.visibilityChangeSubject.OnNext(value);
            }
        }

        public Item[] Results => this.searchService.SearchResults;

        public IObservable<Item[]> WhenResultsChanged => this.searchService.WhenSearchResultsChanged;

        public IObservable<bool> WhenVisibilityChanged => this.visibilityChangeSubject.AsObservable();

        public void OnWrapperClick()
        {
            this.navigationService.Click(this);
        }

        public void OnItemClicked(int itemId)
        {
            this.uriHelper.NavigateTo($"/Item?id={itemId}");
            this.IsVisible = false;
        }
    }
}
