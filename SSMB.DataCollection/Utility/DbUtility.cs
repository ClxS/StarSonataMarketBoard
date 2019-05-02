namespace SSMB.DataCollection.Utility
{
    using System.Linq;
    using Application.Interfaces;
    using Domain;

    internal static class DbUtility
    {
        public static void AddOrUpdateItem(this ISsmbDbContext dbContext, Item item)
        {
            var existingItem = dbContext.Items.FirstOrDefault(i => i.Name == item.Name);
            if (existingItem == null)
            {
                dbContext.Items.Add(item);
            }
            else
            {
                existingItem.Name = item.Name;
                existingItem.Description = item.Description;
                existingItem.StructuredDescription = item.StructuredDescription;
                existingItem.Cost = item.Cost;
                existingItem.Type = item.Type;
                existingItem.Weight = item.Weight;
                existingItem.Space = item.Space;
                existingItem.ScrapValue = item.ScrapValue;
                existingItem.Quality = item.Quality;
            }
        }
    }
}
