namespace SSMB.Domain
{
    using System.ComponentModel;

    public enum AlertField : byte
    {
        [Description("Buy Quantity")]
        BuyQuantity = 0,

        [Description("Sell Quantity")]
        SellQuantity = 1,

        [Description("Buy Price")]
        BuyPrice = 2,

        [Description("Sell Price")]
        SellPrice = 3
    }
}
