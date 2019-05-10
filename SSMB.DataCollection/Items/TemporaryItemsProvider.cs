namespace SSMB.DataCollection.Items
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Domain;
    using UpdateItemListTask;

    class TemporaryItemsProvider : IItemProvider
    {
        public IEnumerable<(string name, ItemType type, long cost, long weight, long space, Quality quality,
            IDictionary<string, string> values)> GetItems()
        {
            string[] lines = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Items/items.txt");
            foreach (var line in lines)
            {
                (string, ItemType, long, long, long, Quality, Dictionary<string, string>)? value = null;
                try
                {
                    var segments = line.Split(",");
                    value = (
                        string.Join(",", segments.Take(segments.Length - 5)),
                        XmlEnumConversions.ItemTypeFromString(segments[^5]),
                        long.Parse(segments[^4]),
                        long.Parse(segments[^3]),
                        long.Parse(segments[^2]),
                        XmlEnumConversions.QualityFromString(segments[^1]),
                        new Dictionary<string, string>());
                }
                catch (Exception)
                {
                    Console.Error.WriteLine($"Failed parsing line: {line}");
                }

                if (value.HasValue)
                {
                    yield return value.Value;
                }
            }
        }
    }
}
