namespace SSMB.Domain
{
    using System.Collections.Generic;

    public class Alert
    {
        public ICollection<AlertCondition> Conditions { get; set; }

        public int Id { get; set; }

        public Item Item { get; set; }

        public int ItemId { get; set; }

        public string Name { get; set; }

        public long UserId { get; set; }
    }
}
