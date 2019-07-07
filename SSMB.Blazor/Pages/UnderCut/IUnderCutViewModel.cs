namespace SSMB.Blazor.Pages.UnderCut
{
    using System;
    using System.Reactive;
    using Application.Items.Models;

    public interface IUnderCutViewModel
    {
        string UnderCutText { get; set; }

        void OnUnderCutClick();

        IObservable<Unit> WhenUnderCutCompleted { get; }

        string SaleText { get; set; }

        string IgnoredText { get; set; }
    }
}