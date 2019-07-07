namespace SSMB.Application.Items.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Domain;

    public class ItemAppraisal
    {
        public enum SellType
        {
            ToBase,
            Scrap
        }

        public Item Item { get; set; }

        public long TotalProfit { get; set; }

        public DateTime DateUpdated { get; set; }

        public IEnumerable<(SellType Type, int Count, string To)> Sales;
    }
}
