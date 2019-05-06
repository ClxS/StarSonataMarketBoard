namespace SSMB.Blazor.Pages.Item
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Application.Items.Models;
    using ViewServices;

    class ItemViewModel : IItemViewModel
    {
        private readonly Subject<FullDetailItem> itemDetailsSubject;
        private readonly IItemsService itemsService;

        private readonly Subject<bool> showStatsChangedSubject;

        private FullDetailItem itemDetail;

        private int? itemId;
        private bool showStats;

        public ItemViewModel(IItemsService itemsService)
        {
            this.itemsService = itemsService;
            this.itemDetailsSubject = new Subject<FullDetailItem>();
            this.showStatsChangedSubject = new Subject<bool>();
        }

        public FullDetailItem ItemDetail
        {
            get => this.itemDetail;
            set
            {
                this.itemDetail = value;
                this.itemDetailsSubject.OnNext(value);
            }
        }

        public int? ItemId
        {
            get => this.itemId;
            set
            {
                this.itemId = value;
                if (value.HasValue)
                {
                    this.itemsService.GetItemDetails(value.Value).ContinueWith(t => { this.ItemDetail = t.Result; });
                }
            }
        }

        public bool ShowStats
        {
            get => this.showStats;
            set
            {
                if (this.showStats.Equals(value))
                {
                    return;
                }

                this.showStats = value;
                this.showStatsChangedSubject.OnNext(value);
            }
        }

        public IObservable<FullDetailItem> WhenItemDetailsUpdated => this.itemDetailsSubject.AsObservable();

        public IObservable<bool> WhenShowStatsChanged => this.showStatsChangedSubject.AsObservable();
    }
}
