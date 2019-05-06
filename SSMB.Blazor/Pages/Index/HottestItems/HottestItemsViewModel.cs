namespace SSMB.Blazor.Shared.HottestItems
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Domain;
    using ViewServices;

    class HottestItemsViewModel : IHottestItemsViewModel
    {
        private readonly Subject<Item[]> itemsSubject = new Subject<Item[]>();
        private Item[] items;

        public HottestItemsViewModel(IItemsService itemsService)
        {
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
    }
}