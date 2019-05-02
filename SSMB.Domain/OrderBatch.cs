namespace SSMB.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class OrderBatch
    {
        public int BatchId { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<OrderEntry> Entries { get; set; }

        public bool IsLtsOrder => this.Entries == null && this.LtsEntries != null;

        public Item Item { get; set; }

        public int ItemId { get; set; }

        public IEnumerable<LtsOrderEntry> LtsEntries { get; set; }

        public IEnumerable<LtsOrderEntry> LtsPurchaseEntries =>
            this.LtsEntries?.Where(e => e.OrderType == OrderType.Purchase);

        public IEnumerable<LtsOrderEntry> LtsSaleEntries =>
            this.LtsEntries?.Where(e => e.OrderType == OrderType.Selling);

        public IEnumerable<OrderEntry> PurchaseEntries => this.Entries?.Where(e => e.OrderType == OrderType.Purchase);

        public IEnumerable<OrderEntry> SaleEntries => this.Entries?.Where(e => e.OrderType == OrderType.Selling);
    }
}
