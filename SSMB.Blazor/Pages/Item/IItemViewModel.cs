namespace SSMB.Blazor.Pages.Item
{
    using System;
    using Application.Items.Models;
    using Domain;

    public interface IItemViewModel
    {
        FullDetailItem ItemDetail { get; set; }

        Alert[] Alerts { get; set; }

        int? ItemId { get; set; }

        bool ShowStats { get; set; }

        bool ShowAlerts { get; set; }

        IObservable<FullDetailItem> WhenItemDetailsUpdated { get; }

        IObservable<Alert[]> WhenAlertsUpdated { get; }
    }
}
