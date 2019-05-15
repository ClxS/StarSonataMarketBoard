namespace SSMB.Blazor.Pages.Item
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Security.Claims;
    using Application.Items.Models;
    using Domain;
    using Microsoft.AspNetCore.Http;
    using ViewServices;

    class ItemViewModel : IItemViewModel
    {
        private readonly Subject<FullDetailItem> itemDetailsSubject;
        private readonly Subject<Alert[]> alertsSubject;
        private readonly IItemsService itemsService;
        private readonly IAlertsService alertsService;
        private readonly IHttpContextAccessor httpContextAccessor;

        private FullDetailItem itemDetail;

        private int? itemId;

        public ItemViewModel(IItemsService itemsService, IAlertsService alertsService, IHttpContextAccessor httpContextAccessor)
        {
            this.itemsService = itemsService;
            this.alertsService = alertsService;
            this.httpContextAccessor = httpContextAccessor;
            this.itemDetailsSubject = new Subject<FullDetailItem>();
            this.alertsSubject = new Subject<Alert[]>();
        }

        public FullDetailItem ItemDetail
        {
            get => this.itemDetail;
            set
            {
                this.itemDetail = value;
                this.itemDetailsSubject.OnNext(value);
            }
        }

        public Alert[] Alerts { get; set; }

        public int? ItemId
        {
            get => this.itemId;
            set
            {
                this.itemId = value;
                if (value.HasValue)
                {
                    this.itemsService.GetItemDetails(value.Value).ContinueWith(t => { this.ItemDetail = t.Result; });

                    var identity = this.httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;
                    if (!(identity?.IsAuthenticated ?? false))
                    {
                        return;
                    }

                    var id = identity.Claims.FirstOrDefault(c => c.Type.EndsWith("nameidentifier"));
                    if (long.TryParse(id?.Value ?? "-", out var idValue))
                    {
                        this.alertsService.GetAlerts(value.Value, idValue).ContinueWith(t =>
                        {
                            this.Alerts = t.Result;
                            this.alertsSubject.OnNext(this.Alerts);
                        });
                    }
                }
            }
        }

        public bool ShowStats { get; set; }

        public bool ShowAlerts { get; set; }

        public IObservable<FullDetailItem> WhenItemDetailsUpdated => this.itemDetailsSubject.AsObservable();

        public IObservable<Alert[]> WhenAlertsUpdated => this.alertsSubject.AsObservable();
    }
}
