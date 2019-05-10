namespace UpdateItemList
{
    using System.IO;
    using System.Text;
    using UpdateItemListTask;

    class Program
    {
        public static void Main(string[] args)
        {
            var sb = new StringBuilder();
            var xmlItemProvider = new XmlItemCollector("Z:\\Projects\\StarSonataSVN\\Dev\\ssserver\\server\\data\\items");
            foreach (var (name, type, cost, weight, space, quality, _) in xmlItemProvider.GetItems())
            {
                sb.AppendLine($"{name},{type},{cost},{weight},{space},{quality}");
            }

            File.WriteAllText("items.txt", sb.ToString());
        }
    }
}
