namespace SSMB.Blazor.Pages.ScrapShop
{
    using System;
    using System.Reactive;

    public interface IScrapShopViewModel
    {
        string BaseText { get; set; }

        IObservable<Unit> WhenScrapShopComplete { get; }
    }
}