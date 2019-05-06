namespace SSMB.Domain
{
    public interface IOrderEntry
    {
        OrderType OrderType { get; set; }

        long Price { get; set; }

        int Quantity { get; set; }

        int BatchId { get; set; }
    }
}