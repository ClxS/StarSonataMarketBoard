namespace SSMB.Blazor.ViewServices
{
    using System;

    public interface INavigationService
    {
        IObservable<object> WhenBubblePreventingElementClicked { get; }

        void Click(object clickedObject);
    }
}
