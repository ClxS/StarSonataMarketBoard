namespace UpdateItemListTask
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    class XmlItemCollector
    {
        private string root;

        public XmlItemCollector(string root)
        {
            this.root = root;
        }

        public IEnumerable<(string name, string type, long cost, long weight, long space, string quality,
            IDictionary<string, string> values)> GetItems()
        {
            foreach (var filePath in Directory.EnumerateFiles(this.root, "*.xml", SearchOption.AllDirectories))
            {
                if (filePath.Contains("aionly") ||
                    filePath.EndsWith("OLD.xml") ||
                    filePath.Contains("exploder.xml") ||
                    filePath.Contains("gearglue.xml") ||
                    filePath.Contains("itemsrename.xml") ||
                    filePath.Contains("slavestasisgens.xml") ||
                    filePath.Contains("spacepointvouchers.xml") ||
                    filePath.Contains("parasite.xml") ||
                    filePath.Contains("spacepoint_items.xml") ||
                    filePath.Contains("index.xml"))
                {
                    continue;
                }

                using var file = File.OpenRead(filePath);
                var settings = new XmlReaderSettings { Async = true };

                using var reader = XmlReader.Create(file, settings);
                var isInInitBlock = false;
                var isInValueBlock = false;
                var outerElement = string.Empty;
                var lastField = string.Empty;

                var name = string.Empty;
                var typeStr = string.Empty;
                var cost = 0L;
                var weight = 0L;
                var space = 0L;
                string quality = null;
                var values = new Dictionary<string, string>();


                while (true)
                {
                    try
                    {
                        if (!reader.Read())
                        {
                            break;
                        }

                        // Early catch for formatting errors in the gingerbread file.
                        var _ = reader.Value;
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
                                outerElement = lastField;
                                if (!outerElement.Equals("MISC"))
                                {
                                    typeStr = outerElement;
                                }

                                isInInitBlock = true;
                            }

                            if (reader.Name.Equals("VALUES"))
                            {
                                isInValueBlock = true;
                            }

                            if (reader.Name.Equals("MISC"))
                            {
                                typeStr = reader.GetAttribute("class");
                            }

                            lastField = reader.Name;
                            break;
                        case XmlNodeType.Text:
                            if (isInInitBlock)
                            {
                                switch (lastField)
                                {
                                    case "NAME":
                                        name = reader.Value;
                                        break;
                                    case "COST":
                                        cost = long.Parse(reader.Value.Replace("_", string.Empty));
                                        break;
                                    case "WEIGHT":
                                        weight = long.Parse(reader.Value.Replace("_", string.Empty));
                                        break;
                                    case "SPACE":
                                        space = (long)(float.Parse(reader.Value.Replace("_", string.Empty)));
                                        break;
                                    case "QUALITY":
                                        quality = reader.Value;
                                        break;
                                }
                            }
                            else if (isInValueBlock)
                            {
                                values[lastField] = reader.Value;
                            }

                            break;
                        case XmlNodeType.EndElement:
                            if (reader.Name.Equals("INIT"))
                            {
                                isInInitBlock = false;
                            }

                            if (reader.Name.Equals("VALUES"))
                            {
                                isInValueBlock = false;
                            }

                            if (reader.Name.Equals(outerElement))
                            {
                                var type = typeStr;
                                yield return (name, type, cost, weight, space,
                                    string.IsNullOrEmpty(quality) ? "COMMON" : quality, values);
                            }

                            break;
                    }
                }
            }
        }
    }
}
