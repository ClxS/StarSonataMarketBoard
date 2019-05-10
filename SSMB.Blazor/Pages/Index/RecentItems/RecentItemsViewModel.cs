namespace SSMB.Blazor.Shared.RecentItems
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Application.Items.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using ViewServices;

    class RecentItemsViewModel : IRecentItemsViewModel
    {
        private readonly IUriHelper uriHelper;
        private readonly Subject<RecentItem[]> resultsChangedSubject;
        private RecentItem[] results;

        public RecentItemsViewModel(IItemsService itemsService, IUriHelper uriHelper)
        {
            this.uriHelper = uriHelper;
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

        public void OnItemClicked(int itemId)
        {
            this.uriHelper.NavigateTo($"/Item?id={itemId}");
        }
    }
}