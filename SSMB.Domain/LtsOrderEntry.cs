namespace SSMB.Domain
{
    public class LtsOrderEntry : IOrderEntry
    {
        public OrderBatch Batch { get; set; }

        public int EntryId { get; set; }

        public OrderType OrderType { get; set; }

        public long Price { get; set; }

        public int Quantity { get; set; }

        public int BatchId { get; set; }
    }
}
