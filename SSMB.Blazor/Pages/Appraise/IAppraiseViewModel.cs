namespace SSMB.Blazor.Pages.Appraise
{
    using System;
    using System.Reactive;
    using Application.Items.Models;

    public interface IAppraiseViewModel
    {
        string AppraiseText { get; set; }

        void OnAppraiseClick();

        IObservable<Unit> WhenAppraisalCompleted { get; }

        ItemAppraisal[] Appraisals { get; }
    }
}