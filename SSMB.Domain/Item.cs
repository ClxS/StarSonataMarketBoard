namespace SSMB.Domain
{
    using System.Collections.Generic;

    public class Item
    {
        public long Cost { get; set; }

        public string Description { get; set; }

        public int ItemId { get; set; }

        public string Name { get; set; }

        public ICollection<OrderBatch> Orders { get; set; }

        public ICollection<Alert> Alerts { get; set; }

        public Quality Quality { get; set; }

        public long ScrapValue { get; set; }

        public int Space { get; set; }

        public string StructuredDescription { get; set; }

        public ItemType Type { get; set; }

        public long Weight { get; set; }
    }
}
