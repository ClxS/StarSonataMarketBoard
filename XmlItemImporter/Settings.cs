namespace XmlItemImporter
{
    using System.Collections.Generic;

    class Settings
    {
        public Dictionary<string, string> ConnectionStrings { get; set; }

        public string XmlRoot { get; set; }
    }
}
