namespace XmlItemImporter
{
    using Microsoft.Extensions.Configuration;

    class Program
    {
        public IConfiguration Configuration { get; }

        /*private static async IAsyncEnumerable<string> GetAllItemNames(string root)
        {
            var counts = new Dictionary<string, int>();
            foreach (var filePath in Directory.EnumerateFiles(root, "*.xml", SearchOption.AllDirectories))
            {
                if (filePath.Contains("aionly"))
                {
                    continue;
                }

                using var file = File.OpenRead(filePath);
                var settings = new XmlReaderSettings { Async = true };

                using var reader = XmlReader.Create(file, settings);
                var isInInitBlock = false;
                var isNextName = false;

                var count = 0;
                while (true)
                {
                    try
                    {
                        if (!await reader.ReadAsync())
                        {
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        // Ignore this file.
                        Console.Error.WriteLine($"Problem reading file {filePath}");
                        break;
                    }

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name.Equals("INIT"))
                            {
                                isInInitBlock = true;
                            }
                            else if (reader.Name.Equals("NAME") && isInInitBlock)
                            {
                                isNextName = true;
                            }

                            break;
                        case XmlNodeType.Text:
                            if (isNextName)
                            {
                                count++;
                                yield return reader.Value;
                                isNextName = false;
                            }

                            break;
                        case XmlNodeType.EndElement:
                            if (reader.Name.Equals("INIT"))
                            {
                                isInInitBlock = false;
                            }

                            break;
                    }
                }

                Debug.WriteLine($"{filePath}: {count}");
                counts[filePath] = count;
            }

            var files = counts.ToArray().OrderByDescending(c => c.Value);
            yield return "";
        }

        private static async IAsyncEnumerable<int> GetNumbersAsync()
        {
            var nums = Enumerable.Range(0, 10);
            foreach (var num in nums)
            {
                await Task.Delay(100);
                yield return num;
            }
        }*/

        static void Main(string[] args)
        {
            /*var settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("AppSettings.json"));

            var optionsBuilder = new DbContextOptionsBuilder<SsmbDbContext>();
            optionsBuilder.UseSqlServer(settings.ConnectionStrings["SSMBDatabase"]);

            var names = new List<string>();
            await foreach (var item in GetAllItemNames(settings.XmlRoot))
            {
                names.Add(item);
            }

            var options = optionsBuilder.Options;
            using var context = new SsmbDbContext(options);
            context.Items.Add(new SSMB.Domain.Item
            {
                Name = "TestItem",
                Description = ""
            });

            context.SaveChanges();*/
        }
    }
}
