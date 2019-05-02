namespace SSMB.Application.Items.Models
{
    using System;
    using Domain;

    public class RecentItem
    {
        public DateTime DateChecked { get; set; }

        public Item Item { get; set; }

        public long? LastHighestPurchasePrice { get; set; }

        public long? LastLowestSalePrice { get; set; }

        public long? PurchaseQuantity { get; set; }

        public long? SaleQuantity { get; set; }
    }
}
