using System;
using System.Collections.Generic;
using System.Text;

namespace SSMB.Application.Items.Models
{
    using Domain;

    public class ItemProfit
    {
        public Item Item { get; set; }

        public long PriceDifferential { get; set; }

        public long FullPotentialProfit { get; set; }

        public string PurchaseStation { get; set; }

        public string SellStation { get; set; }

        public DateTime LastChecked { get; set; }

        public bool ShouldScrap { get; set; }
    }
}
