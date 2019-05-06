namespace SSMB.Application.Items.Models
{
    using System;
    using System.Collections.Generic;
    using Domain;

    public class FullDetailItem
    {
        public Item Item;

        public OrderBatch LatestOrder;

        public List<(DateTime date, long price, long quantity)> PurchasePrices { get; set; }

        public List<(DateTime date, long price, long quantity)> SalePrices { get; set; }
    }
}
