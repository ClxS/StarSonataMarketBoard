namespace SSMB.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class MarketDataStringExtractUtility
    {
        public static string DescriptionFromMarketCheckString(string marketCheckMessage)
        {
            var lines = marketCheckMessage.Split("\n");
            bool hitSellLine = false;
            bool hitDescriptionStart = false;
            string output = string.Empty;
            foreach (var line in lines)
            {
                if (!hitSellLine)
                {
                    if (line.StartsWith("Most profitable locations to sell to:"))
                    {
                        hitSellLine = true;
                    }
                }
                else if (!hitDescriptionStart)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        hitDescriptionStart = true;
                    }
                }
                else
                {
                    output += $"{line}\n";
                }
            }

            return output;
        }

        public static OrderEntry[] OrderDataFromMarketCheckString(string marketCheckMessage)
        {
            return PurchasingFromMarketCheckString(marketCheckMessage)
                   .Concat(SellingFromMarketCheckString(marketCheckMessage)).ToArray();
        }

        public static OrderEntry[] PurchasingFromMarketCheckString(
            string marketCheckMessage)
        {
            var lines = marketCheckMessage.Split('\n');
            var purchaseLines = lines.SkipWhile(l => !l.StartsWith("Most profitable locations")).Skip(1)
                                     .TakeWhile(l => !string.IsNullOrEmpty(l));
            var results = new List<OrderEntry>();
            foreach (var purchaseLine in purchaseLines)
            {
                if (!purchaseLine.Contains("Loc:"))
                {
                    continue;
                }

                GetInformationFromLine(purchaseLine, out var galaxy, out var baseName, out var price, out var quantity,
                    out var isUserbase);
                results.Add(new OrderEntry()
                {
                    GalaxyName = galaxy, BaseName = baseName, Price = price, Quantity = quantity,
                    OrderType = OrderType.Purchase, IsUserBase = isUserbase
                });
            }

            return results.ToArray();
        }

        public static OrderEntry[] SellingFromMarketCheckString(string marketCheckMessage)
        {
            var lines = marketCheckMessage.Split('\n');
            var purchaseLines = lines.SkipWhile(l => !l.StartsWith("Cheapest locations to")).Skip(1)
                                     .TakeWhile(l => !string.IsNullOrEmpty(l));
            var results = new List<OrderEntry>();
            foreach (var purchaseLine in purchaseLines)
            {
                GetInformationFromLine(purchaseLine, out var galaxy, out var baseName, out var price, out var quantity,
                    out var isUserBase);
                results.Add(new OrderEntry()
                {
                    GalaxyName = galaxy, BaseName = baseName, Price = price, Quantity = quantity,
                    OrderType = OrderType.Selling, IsUserBase = isUserBase
                });
            }

            return results.ToArray();
        }

        internal static long ScrapValueFromMarketCheckString(string message)
        {
            var scrapLine = message.Split("\n").FirstOrDefault(l => l.Contains("Scrap Value:"));
            if (scrapLine == null)
            {
                return -1;
            }

            return long.Parse(new string(scrapLine.Where(c => char.IsDigit(c)).ToArray()));
        }

        private static void GetInformationFromLine(string line, out string galaxy, out string baseName,
            out long price, out int quantity, out bool isUserBase)
        {
            var quantityMatch = Regex.Match(line, "[^(]*(?=\\)$)");
            if (quantityMatch.Success)
            {
                quantity = int.Parse(quantityMatch.Value.Replace(",", string.Empty));
                line = line.Replace($"({quantity})", string.Empty);
            }
            else
            {
                quantity = int.MaxValue;
            }

            price = long.Parse(Regex.Match(line, "([\\d|,]*$)").Value.Replace(",", string.Empty));
            galaxy = Regex.Match(line, "(?<=\\s*Loc:\\s)[^:]*").Value;
            baseName = Regex.Match(line, "(?<=Loc:.*:\\s).*(?=(\\s\\s\\sPrice(\\(qty\\)|:)))").Value;
            isUserBase = !baseName.StartsWith("(ai) ");
            if (baseName.StartsWith("(ai) ") || baseName.StartsWith("(ub) "))
            {
                baseName = baseName.Substring(5);
            }
        }
    }
}
