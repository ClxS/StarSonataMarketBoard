namespace SSMB.DataCollection.Items
{
    using System.Collections.Generic;
    using Domain;

    public interface IItemProvider
    {
        IEnumerable<(string name, ItemType type, long cost, long weight, long space, Quality quality, IDictionary<string, string> values)> GetItems();
    }
}
