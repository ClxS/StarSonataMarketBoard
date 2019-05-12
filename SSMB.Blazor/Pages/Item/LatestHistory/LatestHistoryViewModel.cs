namespace SSMB.Blazor.Pages.Item.LatestHistory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Application.Items.Models;
    using Domain;

    class LatestHistoryViewModel : ILatestHistoryViewModel
    {
        private readonly Subject<Unit> orderEntriesUpdated = new Subject<Unit>();
        private IEnumerable<OrderEntry> orderedPurchaseEntries;

        public FullDetailItem ItemDetails { get; private set; }

        public IEnumerable<OrderEntry> OrderedPurchaseEntries
        {
            get => this.orderedPurchaseEntries;
            set
            {
                this.orderedPurchaseEntries = value;
                this.orderEntriesUpdated.OnNext(Unit.Default);
            }
        }

        public OrderType OrderType { get; private set; }

        public IObservable<Unit> WhenOrderedEntriesUpdated => this.orderEntriesUpdated.AsObservable();

        public void SetDetails(OrderType type, FullDetailItem itemDetails)
        {
            this.OrderType = type;
            this.ItemDetails = itemDetails;
            this.OrderedPurchaseEntries = this.GetUnorderedEntries();
        }

        private IEnumerable<OrderEntry> GetUnorderedEntries()
        {
            if (this.ItemDetails?.LatestOrder == null)
            {
                return Enumerable.Empty<OrderEntry>();
            }

            return this.OrderType == OrderType.Purchase
                ? this.ItemDetails.LatestOrder.PurchaseEntries
                : this.ItemDetails.LatestOrder.SaleEntries;
        }
    }
}
