namespace SSMB.Blazor.Pages.Index.HottestItems
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Domain;
    using Microsoft.AspNetCore.Components;
    using ViewServices;

    class HottestItemsViewModel : IHottestItemsViewModel
    {
        private readonly Subject<Item[]> itemsSubject = new Subject<Item[]>();
        private readonly NavigationManager uriHelper;
        private Item[] items;

        public HottestItemsViewModel(IItemsService itemsService, NavigationManager uriHelper)
        {
            this.uriHelper = uriHelper;
            itemsService.GetHotItems(20).ContinueWith(items => { this.Items = items.Result; });
        }

        public Item[] Items
        {
            get => this.items;
            set
            {
                this.items = value;
                this.itemsSubject.OnNext(value);
            }
        }

        public IObservable<Item[]> WhenItemsChanged => this.itemsSubject.AsObservable();

        public void OnItemClicked(int itemId)
        {
            this.uriHelper.NavigateTo($"/Item?id={itemId}");
        }
    }
}
