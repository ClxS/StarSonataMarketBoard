namespace SSMB.Blazor.Pages.Index.ProfitableItems
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Application.Items.Models;
    using Microsoft.AspNetCore.Components;
    using RecentItems;
    using ViewServices;

    class ProfitableItemsViewModel : IProfitableItemsViewModel
    {
        private readonly IUriHelper uriHelper;
        private readonly Subject<ItemProfit[]> resultsChangedSubject;
        private ItemProfit[] results;

        public ProfitableItemsViewModel(IItemsService itemsService, IUriHelper uriHelper)
        {
            this.uriHelper = uriHelper;
            this.resultsChangedSubject = new Subject<ItemProfit[]>();
            itemsService.GetProfitableItems().ContinueWith(items => { this.Results = items.Result; });
        }

        public ItemProfit[] Results
        {
            get => this.results;
            set
            {
                this.results = value;
                this.resultsChangedSubject.OnNext(value);
            }
        }

        public IObservable<ItemProfit[]> WhenResultsChanged => this.resultsChangedSubject.AsObservable();

        public void OnItemClicked(int itemId)
        {
            this.uriHelper.NavigateTo($"/Item?id={itemId}");
        }
    }
}