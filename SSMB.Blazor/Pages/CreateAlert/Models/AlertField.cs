namespace SSMB.Blazor.Pages.CreateAlert.Models
{
    using System.ComponentModel;

    public enum AlertField
    {
        [Description("Buy Quantity")]
        BuyQuantity,

        [Description("Sell Quantity")]
        SellQuantity,

        [Description("Buy Price")]
        BuyPrice,

        [Description("Sell Price")]
        SellPrice
    }
}
