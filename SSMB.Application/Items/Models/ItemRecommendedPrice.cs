namespace SSMB.Application.Items.Models
{
    using Domain;

    public class ItemRecommendedPrice
    {
        public Item Item { get; set; }

        public long Price { get; set; }

        public string Reason { get; set; }
    }
}
