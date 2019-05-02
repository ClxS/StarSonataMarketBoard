namespace SSMB.Blazor.ViewServices.Defaults
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    // This dumb class exists because the Blazor team have never settled on a way to 
    // stop DOM event bubbling, so to achieve click-off behaviour like what we could do in JS with a simple
    // event.stopPropagation(), we have to do this mess.
    // At least they didn't make it an attribute... I hope.
    // https://github.com/aspnet/AspNetCore/issues/5545
    class NavigationService : INavigationService
    {
        private readonly Subject<object> clickedElementSubject;

        private DateTime lastClicked = DateTime.MinValue;

        public NavigationService()
        {
            this.clickedElementSubject = new Subject<object>();
        }

        public IObservable<object> WhenBubblePreventingElementClicked => this.clickedElementSubject.AsObservable();

        public void Click(object clickedObject)
        {
            if ((DateTime.Now - this.lastClicked).Milliseconds < 100)
            {
                return;
            }

            this.lastClicked = DateTime.Now;
            this.clickedElementSubject.OnNext(clickedObject);
        }
    }
}
