namespace SSMB.Blazor.Pages.Item
{
    using System;
    using Application.Items.Models;

    public interface IItemViewModel
    {
        FullDetailItem ItemDetail { get; set; }

        int? ItemId { get; set; }

        bool ShowStats { get; set; }

        IObservable<FullDetailItem> WhenItemDetailsUpdated { get; }

        IObservable<bool> WhenShowStatsChanged { get; }
    }
}
