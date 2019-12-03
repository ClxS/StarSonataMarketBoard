namespace SSMB.Blazor.Pages.UnderCut
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading.Tasks;
    using Application.Items.Models;
    using ViewServices;

    public class UnderCutViewModel : IUnderCutViewModel
    {
        private readonly IItemsService itemsService;
        public string UnderCutText { get; set; }

        private readonly Subject<Unit> underCutCompleted = new Subject<Unit>();
        public IObservable<Unit> WhenUnderCutCompleted => this.underCutCompleted.AsObservable();

        public string SaleText { get; set; }

        public string IgnoredText { get; set; }

        public UnderCutViewModel(IItemsService itemsService)
        {
            this.itemsService = itemsService;
        }

        public void OnUnderCutClick()
        {
            var text = this.UnderCutText.Replace("\r", string.Empty);
            var lines = text.Split("\n");
            var items = new List<string>();
            foreach (var line in lines)
            {
                try
                {
                    var entry = line.Split("	");
                    var name = entry[1];
                    items.Add(name);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            _ = Task.Run(async () =>
            {
                var results = await this.itemsService.GetUnderCut(items.ToArray());
                this.SaleText = "sellprice | buyprice | maxbuy | maxsell | maxmake | name\n";
                this.IgnoredText = string.Empty;
                foreach (var result in results)
                {
                    if (result.Price <= 0)
                    {
                        this.IgnoredText += $"{result.Item.Name} - {result.Reason}\n";
                    }
                    else
                    {
                        this.SaleText += $"{result.Price} | 0 | 0 | 0| 0 | {result.Item.Name}\n";
                    }
                }

                this.underCutCompleted.OnNext(Unit.Default);
            });
        }
    }
}
