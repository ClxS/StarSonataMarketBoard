namespace SSMB.Blazor.Pages.ScrapShop
{
    using System;
    using System.Reactive;
    using System.Reactive.Subjects;
    using System.Threading.Tasks;
    using ViewServices;

    public class ScrapShopViewModel : IScrapShopViewModel
    {
        public ScrapShopViewModel(IItemsService itemsService)
        {
            this.scrapShopCompleteSubject = new Subject<Unit>();
            _ = Task.Run(async () =>
            {
                var results = await itemsService.GetScrapList();
                this.BaseText = "sellprice | buyprice | maxbuy | maxsell | maxmake | name\n";
                foreach (var result in results)
                {
                    this.BaseText += $"0 | {result.Price} | 10000 | 0| 0 | {result.Item.Name}\n";
                }

                this.scrapShopCompleteSubject.OnNext(Unit.Default);
            });
        }

        public string BaseText { get; set; }

        public IObservable<Unit> WhenScrapShopComplete => this.scrapShopCompleteSubject;
        private readonly Subject<Unit> scrapShopCompleteSubject;
    }
}
