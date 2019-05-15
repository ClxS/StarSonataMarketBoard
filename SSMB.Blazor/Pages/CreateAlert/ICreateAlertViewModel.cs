namespace SSMB.Blazor.Pages.CreateAlert
{
    using System;
    using System.Collections.Generic;
    using Application.Items.Models;
    using Domain;

    public interface ICreateAlertViewModel
    {
        FullDetailItem ItemDetail { get; set; }

        int? ItemId { get; set; }

        string AlertName { get; set; }

        IObservable<FullDetailItem> WhenItemDetailsUpdated { get; }

        IList<AlertCondition> Conditions { get; }

        void AddConditionClicked();

        void AddAlertClicked();

        void RemoveCondition(int conditionIndex);
    }
}
