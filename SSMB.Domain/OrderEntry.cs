namespace SSMB.Domain
{
    public class OrderEntry : IOrderEntry
    {
        public string BaseName { get; set; }

        public OrderBatch Batch { get; set; }

        public int EntryId { get; set; }

        public string GalaxyName { get; set; }

        public bool IsUserBase { get; set; }

        public OrderType OrderType { get; set; }

        public long Price { get; set; }

        public int Quantity { get; set; }

        public int BatchId { get; set; }
    }
}
